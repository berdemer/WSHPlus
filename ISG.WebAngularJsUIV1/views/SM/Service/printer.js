'use strict';

angular.module('inspinia')
     .service('messagingService', ['$rootScope', '$timeout', function ($rootScope, $timeout) {
         this.messages = {error: [], info: []};
         var self = this;

         $rootScope.$on('event:serverError', function (event, errorMessage) {
             self.showMessage('error', errorMessage, 'serverError');
         });

         this.showMessage = function (level, message, errorEvent) {
             var messageObject = {'value': '', 'isServerError': false};
             messageObject.value = message;
             if (errorEvent) {
                 messageObject.isServerError = true;
             } else if (level === 'info') {
                 this.createTimeout('info', 4000);
             }

             var index = _.findIndex(this.messages[level], function (msg) {
                 return msg.value === messageObject.value;
             });

             if (index >= 0) {
                 this.messages[level].splice(index, 1);
             }
             this.messages[level].push(messageObject);
         };

         this.createTimeout = function (level, time) {
             $timeout(function () {
                 self.messages[level] = [];
             }, time, true);
         };

         this.hideMessages = function (level) {
             self.messages[level].length = 0;
         };
     }])
    .factory('spinner', ['messagingService', '$timeout', function (messagingService, $timeout) {
        var tokens = [];

        var topLevelDiv = function (element) {
            return $(element).find("div").eq(0);
        };

        var showSpinnerForElement = function (element) {
            if ($(element).find(".dashboard-section-loader").length === 0) {
                topLevelDiv(element)
                    .addClass('spinnable')
                    .append('<div class="dashboard-section-loader"></div>');
            }
            return {
                element: $(element).find(".dashboard-section-loader")
            };
        };

        var showSpinnerForOverlay = function () {
            var token = Math.random();
            tokens.push(token);

            if ($('#overlay').length === 0) {
                $('body').prepend('<div id="overlay"><div></div></div>');
            }

            var spinnerElement = $('#overlay');
            spinnerElement.stop().show();

            return {
                element: spinnerElement,
                token: token
            };
        };

        var show = function (element) {
            messagingService.hideMessages("error");
            if (element !== undefined) {
                return showSpinnerForElement(element);
            }

            return showSpinnerForOverlay();
        };

        var hide = function (spinner, parentElement) {
            var spinnerElement = spinner.element;
            if (spinner.token) {
                _.pull(tokens, spinner.token);
                if (tokens.length === 0) {
                    spinnerElement.fadeOut(300);
                }
            } else {
                topLevelDiv(parentElement).removeClass('spinnable');
                spinnerElement && spinnerElement.remove();
            }
        };

        var forPromise = function (promise, element) {
            return $timeout(function () {
                // Added timeout to push a new event into event queue. So that its callback will be invoked once DOM is completely rendered
                var spinner = show(element);                      // Don't inline this element
                promise['finally'](function () {
                    hide(spinner, element);
                });
                return promise;
            });
        };

        var forAjaxPromise = function (promise, element) {
            var spinner = show(element);
            promise.always(function () {
                hide(spinner, element);
            });
            return promise;
        };

        return {
            forPromise: forPromise,
            forAjaxPromise: forAjaxPromise,
            show: show,
            hide: hide
        };
    }])
    .factory('printer', ['$rootScope', '$compile', '$http', '$timeout', '$q', 'spinner',
        function ($rootScope, $compile, $http, $timeout, $q, spinner) {
            var printHtml = function (html) {
                var deferred = $q.defer();
                var hiddenFrame = $('<iframe style="display: none"></iframe>').appendTo('body')[0];
                hiddenFrame.contentWindow.printAndRemove = function () {
                    hiddenFrame.contentWindow.print();
                    $(hiddenFrame).remove();
                    deferred.resolve();
                };
                var htmlContent = "<!doctype html>" +
                        "<html  moznomarginboxes mozdisallowselectionprint>" +
                            '<body onload="window.print();">' +
                                html +
                            '</body>' +
                        "</html>";
                var doc = hiddenFrame.contentWindow.document.open("text/html", "replace");
                doc.write(htmlContent);
                doc.close();
                return deferred.promise;
            };

            /*                        '<html  moznomarginboxes mozdisallowselectionprint> <style type="text/css"  media="print">' +
                        '@media print {body * {visibility: hidden;}.yaz * {visibility: visible;}.yaz { position: absolute;display: block !important;'+
                        ' width: auto;height: 99%;left: 0; top: 0; } }</style>' +
                        '<body onload="window.print();">' +*/

            var openNewWindow = function (html) {
                var newWindow = window.open("printTest.html");
                newWindow.addEventListener('load', function () {
                    $(newWindow.document.body).html(html);
                }, false);
            };

            var print = function (templateUrl, data) {
                $rootScope.isBeingPrinted = true;
                $http.get(templateUrl).then(function (templateData) {
                    var template = templateData.data;
                    var printScope = $rootScope.$new();
                    angular.extend(printScope, data);
                    var element = $compile($('<div>' + template + '</div>'))(printScope);
                    var renderAndPrintPromise = $q.defer();
                    var waitForRenderAndPrint = function () {
                        if (printScope.$$phase || $http.pendingRequests.length) {
                            $timeout(waitForRenderAndPrint, 1000);
                        } else {
                            printHtml(element.html()).then(function () {
                                $rootScope.isBeingPrinted = false;
                                renderAndPrintPromise.resolve();
                            });
                            printScope.$destroy();
                        }
                        return renderAndPrintPromise.promise;
                    };
                    spinner.forPromise(waitForRenderAndPrint());
                });
            };

            var printFromScope = function (templateUrl, scope, afterPrint) {
                $rootScope.isBeingPrinted = true;
                $http.get(templateUrl).then(function (response) {
                    var template = response.data;
                    var printScope = scope;
                    var element = $compile($('<div>' + template + '</div>'))(printScope);
                    var renderAndPrintPromise = $q.defer();
                    var waitForRenderAndPrint = function () {
                        if (printScope.$$phase || $http.pendingRequests.length) {
                            $timeout(waitForRenderAndPrint);
                        } else {
                            printHtml(element.html()).then(function () {
                                $rootScope.isBeingPrinted = false;
                                if (afterPrint) {
                                    afterPrint();
                                }
                                renderAndPrintPromise.resolve();
                            });
                        }
                        return renderAndPrintPromise.promise;
                    };
                    spinner.forPromise(waitForRenderAndPrint());
                });
            };
            return {
                print: print,
                printFromScope: printFromScope
            };
        }]);