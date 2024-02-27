'use strict';
function allerjiHastaTakibiCtrl($scope, SMSvc, DTOptionsBuilder, DTColumnDefBuilder, analizSvc, personellerSvc) {
	var ahtC = this;
	$scope.dtOptions = DTOptionsBuilder.newOptions()
	 .withLanguageSource('/views/Personel/Controller/Turkish.json')
	 .withDOM('<"html5buttons"B>lTfgitp')
	 .withButtons([
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Yıllık Değerlendirme Raporu', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Yıllık Değerlendirme Raporu', titleAttr: 'PDF' },
		 {
			 extend: 'print', text: '<i class="fa fa-print"></i>', titleAttr: 'Yazdır',
			 customize: function (win) {
				 $(win.document.body).addClass('white-bg');
				 $(win.document.body).css('font-size', '10px');
				 $(win.document.body).find('table')
					 .addClass('compact')
					 .css('font-size', 'inherit');
			 }
		 }
	 ])
	 .withPaginationType('full')
	 .withSelect(true)
	 .withOption('lengthMenu', [20, 30, 50, 100])
	 .withOption('order', [1, 'ahtC'])
     .withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefs = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];



    personellerSvc.MoviesIds.SirketId = 0;
    personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';

    $scope.SirketBasligi = {
        id: 0,
        name: 'Şirketi Seçiniz.!'
    };
    personellerSvc.GetSirketler(true).then(function (data) {
        $scope.collectionSirket = data;
        addToAllNodes(data);
        $scope.expandList = allNodes;
    });


    var allNodes = [];
    function addToAllNodes(children) {
        if (!children || Array.isArray(children) && children.length === 0) {
            return;
        }
        for (var i = 0; i < children.length; i++) {
            allNodes.push(children[i]);
            addToAllNodes(children[i].children);
        }
    }
    $scope.status = {
        open: false
    };
    $scope.showSelected = function (sel) {
        $scope.SirketBasligi.name = sel.name;
        $scope.SirketBasligi.id = sel.id;
        personellerSvc.MoviesIds.SirketId = sel.id;//ŞİRKETİN ID BİLGİSİ
        personellerSvc.MoviesIds.SirketAdi = sel.name;
        $scope.status.open = !$scope.status.open;
    };
    $scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
        $scope.SirketBasligi.name = personellerSvc.MoviesIds.SirketAdi;
        $scope.SirketBasligi.id = personellerSvc.MoviesIds.SirketId;
        raporlar($scope.SirketBasligi.id);
        if (angular.isUndefined(newValue)) {
            $scope.SirketBasligi = {
                id: 0,
                name: 'Şirketi Seçiniz.!'
            };
        }
        ahtC.sirketAdi = $scope.SirketBasligi.id === 0 ? '' : $scope.SirketBasligi.name;
    });

    var raporlar = function (Sirket_Id) {
        analizSvc.AllerjiHastaTakibi(Sirket_Id).then(function (resp) {
            ahtC.raporlar = resp;
        });
    };

}

allerjiHastaTakibiCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'analizSvc', 'personellerSvc'];

angular
	.module('inspinia')
    .controller('allerjiHastaTakibiCtrl', allerjiHastaTakibiCtrl);