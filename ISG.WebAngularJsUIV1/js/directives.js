/**
 * INSPINIA - Responsive Admin Theme
 *
 */


/**
 * pageTitle - Directive for set Page title - mata title
 */
function pageTitle($rootScope, $timeout) {
    return {
        link: function (scope, element) {
            var listener = function (event, toState, toParams, fromState, fromParams) {
                // Default title - load on Dashboard 1
                var title = 'WSH++ | Admin Sayfa Bilgisi';
                // Create your own title pattern
                if (toState.data && toState.data.pageTitle) title = 'WSH++ | ' + toState.data.pageTitle;
                $timeout(function () {
                    element.text(title);
                });
            };
            $rootScope.$on('$stateChangeStart', listener);
        }
    }
}

/**
 * sideNavigation - Directive for run metsiMenu on sidebar navigation
 */
function sideNavigation($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            // Call the metsiMenu plugin and plug it to sidebar navigation
            $timeout(function () {
                element.metisMenu();
            });
        }
    };
}

 function onError () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            element.on('error', function () {
                element.attr('src', attr.onError);
            })
        }
    }
};

function iboxTools($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: 'views/common/ibox_tools.html',
        controller: function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var ibox = $element.closest('div.ibox');
                var icon = $element.find('i:first');
                var content = ibox.find('div.ibox-content');
                content.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                ibox.toggleClass('').toggleClass('border-bottom');
                $timeout(function () {
                    ibox.resize();
                    ibox.find('[id^=map-]').resize();
                }, 75);
            };
            // Function for close ibox
            $scope.closebox = function () {
                var ibox = $element.closest('div.ibox');
                ibox.remove();
            }
        }
    };
}



function iboxToolsFullScreen($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: 'views/common/ibox_tools_full_screen.html',
        controller: function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var ibox = $element.closest('div.ibox');
                var icon = $element.find('i:first');
                var content = ibox.find('div.ibox-content');
                content.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                ibox.toggleClass('').toggleClass('border-bottom');
                $timeout(function () {
                    ibox.resize();
                    ibox.find('[id^=map-]').resize();
                }, 75);
            };
            // Function for close ibox
            $scope.closebox = function () {
                var ibox = $element.closest('div.ibox');
                ibox.remove();
            };
            // Function for full screen
            $scope.fullscreen = function () {
                var ibox = $element.closest('div.ibox');
                var button = $element.find('i.fa-expand');
                $('body').toggleClass('fullscreen-ibox-mode');
                button.toggleClass('fa-expand').toggleClass('fa-compress');
                ibox.toggleClass('fullscreen');
                setTimeout(function () {
                    $(window).trigger('resize');
                }, 100);
            }
        }
    };
}

/**
 * minimalizaSidebar - Directive for minimalize sidebar
*/
function minimalizaSidebar($timeout) {
    return {
        restrict: 'A',
        template: '<a class="navbar-minimalize minimalize-styl-2 btn btn-primary " href="" ng-click="minimalize()"><i class="fa fa-bars"></i></a>',
        controller: function ($scope, $element) {
            $scope.minimalize = function () {
                $("body").toggleClass("mini-navbar");
                if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
                    // Hide menu in order to smoothly turn on when maximize menu
                    $('#side-menu').hide();
                    // For smoothly turn on menu
                    setTimeout(
                        function () {
                            $('#side-menu').fadeIn(500);
                        }, 100);
                } else if ($('body').hasClass('fixed-sidebar')) {
                    $('#side-menu').hide();
                    setTimeout(
                        function () {
                            $('#side-menu').fadeIn(500);
                        }, 300);
                } else {
                    // Remove all inline style from jquery fadeIn function to reset menu state
                    $('#side-menu').removeAttr('style');
                }
            }
        }
    };
}

function myMessages() {
    return {
        templateUrl: 'custom-messages.html',
        scope: { options: '=myMessages' }
    };
}

function flip() {
    return {
        restrict: "A",
        scope: true,
        link: function (scope, element) {
            var $panels = element.css({ position: 'relative' }).children().addClass("flip-panel");
            var frontPanel = $panels.eq(0);
            var backPanel = $panels.eq(1);

            scope.showFrontPanel = function () {
                frontPanel.removeClass("flip-hide-front-panel");
                backPanel.addClass("flip-hide-back-panel");
            };

            scope.showBackPanel = function () {
                backPanel.removeClass("flip-hide-back-panel");
                frontPanel.addClass("flip-hide-front-panel");
            };

            scope.showFrontPanel();
        }
    }
};

function resize($window) {
    return {
        link: link,
        restrict: 'A'
    };

    function link(scope, element, attrs) {
        scope.width = $window.innerWidth;
        function onResize() {
            // uncomment for only fire when $window.innerWidth change   
            if (scope.width !== $window.innerWidth) {
                scope.width = $window.innerWidth;
                scope.$digest();
            }
        };

        function cleanUp() {
            angular.element($window).off('resize', onResize);
        }

        angular.element($window).on('resize', onResize);
        scope.$on('$destroy', cleanUp);
    }
};

function uppercased() {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (input) {
                return input ? input.toUpperCase() : "";
            });
            element.css("text-transform", "uppercase");
        }
    };
};

function arrowSelector($document) {
    return {
        restrict: 'A',
        link: function (scope, elem, attrs, ctrl) {
            var elemFocus = false;
            elem.on('mouseenter', function () {
                elemFocus = true;
            });
            elem.on('mouseleave', function () {
                elemFocus = false;
            });
            $document.bind('keydown', function (e) {
                if (elemFocus) {
                    if (e.keyCode == 38) {
                        if (scope.selectedRow == 0) {
                            return;
                        }
                        scope.selectedRow--;
                        scope.$apply();
                        e.preventDefault();
                    }
                    if (e.keyCode == 40) {
                        if (scope.selectedRow == scope.foodItems.length - 1) {
                            return;
                        }
                        scope.selectedRow++;
                        scope.$apply();
                        e.preventDefault();
                    }
                }
            });
        }
    };
};

function ngEnter() {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        })
    }
}

function clockPicker() {
    return {
        restrict: 'A',
        link: function (scope, element) {
            element.clockpicker();
        }
    };
};

function fullScroll($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            $timeout(function () {
                element.slimscroll({
                    height: '100%',
                    railOpacity: 0.9
                });

            });
        }
    };
}

/**
 * slimScroll - Directive for slimScroll with custom height
 */
function slimScroll($timeout) {
    return {
        restrict: 'A',
        scope: {
            boxHeight: '@'
        },
        link: function (scope, element) {
            $timeout(function () {
                element.slimscroll({
                    height: scope.boxHeight,
                    railOpacity: 0.9
                });

            });
        }
    };
}



function autoComplete() {
    return function ($timeout) {
        return function (scope, iElement, iAttrs) {
            iElement.autocomplete({
                source: scope[iAttrs.uiItems],
                select: function () {
                    $timeout(function () {
                        iElement.trigger('input');
                    }, 0);
                }
            });
        };
    };
};


function directive() {
    return {
        restrict: 'A',
        scope: {

            options: '=', //an object structured like below
            /*
                {
                    rotation: int for rotation clockwise in degrees
                    scaling: float for scaling factor
                    originalSize: {height, width} of image (optional)
                }
               */

        },
        link: link
    };

    function link(scope, element, attributes) {
        element.bind('load', function () {
            if (!scope.options.originalSize) {
                element.removeAttr('style'); //clear all previous styling

                //workaround for IE (it's dumb, and I'd rather just use this element (element[0]) data)
                var img = document.createElement('img');
                img.src = element[0].src;
                scope.options.originalSize = {
                    height: img.height,
                    width: img.width
                };
                scope.options.scaling = 1.0;
                scope.options.rotation = 0;
            }
            transformWithCss();
        });

        scope.$watch('options.rotation', transformWithCss);
        scope.$watch('options.scaling', transformWithCss);

        function transformWithCss() {
            if (!scope.options || !scope.options.originalSize)
                return;

            var width = scope.options.originalSize.width * scope.options.scaling;
            var height = scope.options.originalSize.height * scope.options.scaling;
            var marginTop, marginLeft;

            var effectiveRotation = (scope.options.rotation % 360 + 360) % 360;
            switch (effectiveRotation) {
                case 0:
                    marginTop = 0;
                    marginLeft = 0;
                    break;
                case 90:
                    marginTop = 0;
                    marginLeft = height * scope.options.scaling;
                    break;
                case 180:
                    marginTop = height * scope.options.scaling;
                    marginLeft = width * scope.options.scaling;
                    break;
                case 270:
                    marginTop = width * scope.options.scaling;
                    marginLeft = 0;
                    break;
                default:
                    //how did we get here? throw exception?
                    alert("something went wrong with rotation");
                    break;
            }

            element.css({
                "transform": 'scale(' + scope.options.scaling + ') rotate(' + scope.options.rotation + 'deg) ',
                "width": width + 'px',
                "height": height + 'px',
                "transform-origin": '0px 0px',
                "margin-top": marginTop + 'px',
                "margin-left": marginLeft + 'px'
            });
        }

    }
}

angular
    .module('inspinia')
    .directive('pageTitle', pageTitle)
    .directive('sideNavigation', sideNavigation)
    .directive('iboxTools', iboxTools)
    .directive('minimalizaSidebar', minimalizaSidebar)
    .directive('myMessages', myMessages)
    .directive('iboxToolsFullScreen', iboxToolsFullScreen)
    .directive('flip', flip)
    .directive('resize', resize)
    .directive('arrowSelector', arrowSelector)
    .directive('uppercased', uppercased)
    .directive('ngEnter', ngEnter)
    .directive('clockPicker', clockPicker)
    .directive('autoComplete', autoComplete)
    .directive('fullScroll', fullScroll)
    .directive('slimScroll', slimScroll)
    .directive('onError', onError)
    .directive('scalingRotatingImage', directive)
;