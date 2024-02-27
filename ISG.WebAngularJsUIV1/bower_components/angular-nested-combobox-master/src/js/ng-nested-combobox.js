(function (window, angular, undefined) {
    'use strict';

    angular.module('ui.nested.combobox', [])
        .constant('nestedComboBoxConfig', {
            options: {
                childrenParam: 'children'
            }
        })
        .controller('NestedComboBoxController', ['$scope', '$element', '$attrs', 'nestedComboBoxConfig', '$timeout', function ($scope, $element, $attrs, nestedComboBoxConfig, $timeout) {
            'use strict';
            var gs = this,
                oldMemberId = null;
            this.isOpen = false;
            this.options = angular.isDefined($scope.options) ? $scope.options : nestedComboBoxConfig.options;
            this.selectedItem = {};
            var node = false;

            $scope.$watch('collection', function(value){
                if($scope.collection){
                    if(!angular.isArray($scope.collection)){
                        $scope.collection = [$scope.collection];
                    }
                    for(var y = 0; y < $scope.collection.length; y += 1 ) {
                        node = findNode($scope.nsNgModel, $scope.collection[y]);
                        if(node !== false){
                            angular.extend(gs.selectedItem, node);
                        }
                    }
                }
            });


            this.toggleOpen = function () {
                if ($scope.controlDisabled) {
                    this.isOpen = false;
                    return false;
                }
                this.isOpen = !this.isOpen;
            };

            this.toggleBlur = function() {
                $timeout(function (){
                    gs.isOpen = false;
                }, 200);
            };

            this.toggleFocus = function(){
                if ($scope.controlDisabled) {
                    this.isOpen = false;
                    return false;
                }
                gs.isOpen = false;
            };

            this.selectValue = function (event, member) {

                if (oldMemberId === member.id) {
                    return true;
                }
                
                $scope.changeEvent(member);
                angular.extend(gs.selectedItem, member);
                $scope.nsNgModel = member.id;
                oldMemberId = member.id;

            };

            function findNode(id, currentNode) {
                var i,
                    currentChild,
                    result;

                    if (id === currentNode.id) {
                        return currentNode;
                    } else {

                        if (angular.isArray(currentNode[gs.options.childrenParam])) {
                            for (i = 0; i < currentNode[gs.options.childrenParam].length; i += 1) {
                                currentChild = currentNode[gs.options.childrenParam][i];
                                // Search in the current child
                                result = findNode(id, currentChild);

                                // Return the result if the node has been found
                                if (result !== false) {
                                    return result;
                                }
                            }
                        }
                        // The node has not been found and we have no more options
                        return false;
                    }

            }
        }])
        .directive('nestedComboBox', ['$templateCache', function ($templateCache) {
            'use strict';

            return {
                restrict: 'E',
                controller: 'NestedComboBoxController',
                controllerAs: 'gs',
                replace: true,
                template: $templateCache.get('select-group.html'),
                scope: {
                    collection: '=?',
                    controlClass: '@?',
                    controlDisabled: '=?',
                    changeEvent: '=?',
                    options: '=?',
                    nsNgModel: '=?'
                }
            };
        }]);
})(window, window.angular);