'use strict';
function revirIslemleriCtrl($scope, SMSvc, DTOptionsBuilder, DTColumnDefBuilder, analizSvc) {
	var riC = this;

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
	 .withOption('order', [1, 'asc'])
		.withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefs = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];


    var raporlar = function (Sirket_Id, tarih) {
        var month = tarih.getMonth() + 1;
        var date = ('0' + tarih.getDate()).slice(-2);
        var year = tarih.getFullYear();
        analizSvc.RevirIslemleri(Sirket_Id, date, month,year).then(function (resp) {
            riC.personeller = resp;
		});
	};

    riC.AylikListe = function (Sirket_Id, tarih) {
        var month = tarih.getMonth() + 1;
        var date = ('0' + tarih.getDate()).slice(-2);
        var year = tarih.getFullYear();
        analizSvc.AylikRevirIslemleri(Sirket_Id, date, month, year).then(function (resp) {
            riC.personeller = resp;
        });
    };

	SMSvc.AllSbirimleri().then(function (data) {
		$scope.birimler = data;
		$scope.birim = { selected: data[0] };
        raporlar($scope.birim.selected.SaglikBirimi_Id, new Date());
	});

	$scope.$watch(function () { return riC.tarih; }, function (newValue, oldValue) {//tarih değişiklerini ilet
		if (newValue !== oldValue) {raporlar($scope.birim.selected.SaglikBirimi_Id,newValue);
		}
	});

	$scope.$watch(function () { return $scope.birim.selected.SaglikBirimi_Id; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			raporlar(newValue,riC.tarih);
		}
    });

    $scope.today = function () {
        riC.tarih = new Date();
    };

    $scope.today();

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.popup1 = {
        opened: false
    };
}

revirIslemleriCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'analizSvc'];

angular
	.module('inspinia')
    .controller('revirIslemleriCtrl', revirIslemleriCtrl);