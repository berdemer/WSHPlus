'use strict';
function sirketBolumleriCrtl($scope, sirketSvc, $stateParams) {

    var _sirketBolumu = {
        id: '',
        idRef: '',
        bolumAdi: '',
        status: true,
        sirket_Id: ''
    };

    $scope.getSirketBolumuAllData = function (boole) {
        sirketSvc.GetSirketBolumu(boole, $stateParams.id).then(function (data) {
            $scope.data2 = data;
        });
    };
     $scope.getSirketBolumuAllData(true);

    $scope.treeOptions2 = {
        accept: function (sourceNodeScope, destNodesScope, destIndex) {
            return true;
        },
        dropped: function (e) {
            var nodeData = e.source.nodeScope.$modelValue;//aktif dugumu
            var newNodeData = e.dest.nodesScope.$parent.$modelValue;//yeni ust dugumu
            //console.log(e.source.nodeScope.$parentNodeScope.$modelValue);//eski alt dugumu
            _sirketBolumu = {
                id: nodeData.tabloId,
                idRef: newNodeData === undefined ? 0 : newNodeData.tabloId,
                bolumAdi: nodeData.Text,
                status: true,
                sirket_Id: $scope.sirket.id
            };
            sirketSvc.UpdateSirketBolumu(nodeData.tabloId, _sirketBolumu).then(function (response) {
                $scope.getSirketBolumuAllData(true);
            }, function (err) {
                if (err.field && err.msg) {
                } else {
                }
            });
        }
    };

    $scope.saveSirketBolumu = function (scope) {
        var nodeData = scope.$modelValue;
        _sirketBolumu = {
            id: nodeData.tabloId,
            idRef: nodeData.Depth,
            bolumAdi: nodeData.Text,
            status: true,
            sirket_Id: $stateParams.id
        };
        sirketSvc.UpdateSirketBolumu(nodeData.tabloId, _sirketBolumu).then(function (response) {
            $scope.getSirketBolumuAllData(true);
        }, function (err) {
            if (err.field && err.msg) {
                // $scope.editableForm.$setError(err.field, err.msg);
            } else {
                //  $scope.editableForm.$setError('Giriş', 'Unknown error!');
            }
        });
    };

    $scope.passiveBolumStatus = function (scope) {
        var nodeData = scope.$modelValue;
        _sirket = {
            id: nodeData.tabloId,
            idRef: nodeData.Depth,
            bolumAdi: nodeData.Text,
            status: nodeData.status === true ? false : true,
            sirket_Id: $stateParams.id
        };
        sirketSvc.UpdateSirketBolumu(nodeData.tabloId, _sirketBolumu).then(function (response) {
        }, function (error) { console.log(error); });
    };

    $scope.newSubItemSirketBolumu = function (scope) {//yeni kayıt
        var nodeData = scope.$modelValue;
        _sirketBolumu = {
            id: '',
            idRef: nodeData.tabloId,
            bolumAdi: nodeData.Text + '/Güncelleyiniz.',
            status: true,
            sirket_Id: $stateParams.id
        };
        sirketSvc.InsertSirketBolumu(_sirketBolumu).then(function (response) {
            $scope.getSirketBolumuAllData(true);
        }, function (err) {
            if (err.field && err.msg) {
            } else {
            }
        });
    };

    $scope.newSubSirketBolumu = function () {//yeni kayıt
        _sirketBolumu = {
            id: '',
            idRef: 0,
            bolumAdi: 'Yeni Bölüm-Kısım/Güncelleyiniz.',
            status: true,
            sirket_Id: $stateParams.id
        };
        sirketSvc.InsertSirketBolumu(_sirketBolumu).then(function (response) {
            $scope.getSirketBolumuAllData(true);
        });
    };

    var getRootNodesScope2 = function () {
        return angular.element(document.getElementById("tree-root2")).scope();
    };

    $scope.collapseAll2 = function () {
        var scope = getRootNodesScope2();
        scope.collapseAll();
    };

    $scope.expandAll2 = function () {
        var scope = getRootNodesScope2();
        scope.expandAll();
    };

}

sirketlerCrtl.$inject = ['$scope', 'sirketSvc', ' $stateParams'];

angular
    .module('inspinia')
    .controller('sirketlerCrtl', sirketlerCrtl);