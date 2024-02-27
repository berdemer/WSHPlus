'use strict';
function yillikRaporCtrl($scope, SMSvc, DTOptionsBuilder, DTColumnDefBuilder, analizSvc) {
	var yrC = this;

	yrC.year = (new Date().getFullYear()).toString();

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

	yrC.toplam = function (ay) {
		return ay.Ocak + ay.Subat + ay.Mart + ay.Nisan + ay.Mayis + ay.Haziran + ay.Temmuz + ay.Agustos + ay.Eylul + ay.Ekim + ay.Kasim + ay.Aralik;
	};

	var raporlar = function (year, sbId) {
		analizSvc.YillikDegerlendirme(year, sbId).then(function (resp) {
			yrC.aylar = resp;
		});
	};

	SMSvc.AllSbirimleri().then(function (data) {
		$scope.birimler = data;
		$scope.birim = { selected: data[0] };
		raporlar(yrC.year, $scope.birim.selected.SaglikBirimi_Id);
	});

	$scope.$watch(function () { return yrC.year; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			raporlar(newValue, $scope.birim.selected.SaglikBirimi_Id);
		}
	});

	$scope.$watch(function () { return $scope.birim.selected.SaglikBirimi_Id; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			raporlar(yrC.year, newValue);
		}
	});

	yrC.lineOptions = {
		multiTooltipTemplate: "<%=datasetLabel%> : <%=value%>",
		scaleShowGridLines: true,
		scaleGridLineColor: "rgba(0,0,0,.05)",
		scaleGridLineWidth: 1,
		bezierCurve: true,
		bezierCurveTension: 0.4,
		pointDot: true,
		pointDotRadius: 4,
		pointDotStrokeWidth: 1,
		pointHitDetectionRadius: 20,
		datasetStroke: true,
		datasetStrokeWidth: 2,
		datasetFill: true
	};

	$scope.$watch(function () { return yrC.aylar; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			var labels = ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'];
			var datasets = [];
			yrC.series = [];
			angular.forEach(newValue, function (x) {
				var gui = generateUUID();
				var c1 = Math.floor(Math.random() * 256);
				var c2 = Math.floor(Math.random() * 256);
				var c3 = Math.floor(Math.random() * 256);
				yrC.series.push(x.IslemDetayi);
				datasets.push({
					gui:gui,
					label: x.IslemDetayi,
					fillColor: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",0.5)",
					strokeColor: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",1)",
					pointColor: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",1)",
					pointStrokeColor: "#fff",
					pointHighlightFill: "#fff",
					pointHighlightStroke: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",1)",
					data: [x.Ocak, x.Subat, x.Mart, x.Nisan, x.Mayis, x.Haziran, x.Temmuz, x.Agustos, x.Eylul, x.Ekim, x.Kasim, x.Aralik]
				});
			});
			yrC.datasets = datasets;
			yrC.lineData = {
				labels: labels,
				datasets: []
			};
		}
	});

	function generateUUID() {
		var d = new Date().getTime();
		if (window.performance && typeof window.performance.now === "function") {
			d += performance.now(); //use high-precision timer if available
		}
		var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
			var r = (d + Math.random() * 16) % 16 | 0;
			d = Math.floor(d / 16);
			return (c === 'x' ? r : r & 0x3 | 0x8).toString(16);
		});
		return uuid;
	}

	yrC.lineAddtem = function (gui,i) {
		if (gui) {
			yrC.lineData.datasets.push(i);
		} else {
			var index = yrC.lineData.datasets.indexOf(i);
			yrC.lineData.datasets.splice(index, 1);
		}
	};
}

yillikRaporCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'analizSvc'];

angular
	.module('inspinia')
	.controller('yillikRaporCtrl ', yillikRaporCtrl);