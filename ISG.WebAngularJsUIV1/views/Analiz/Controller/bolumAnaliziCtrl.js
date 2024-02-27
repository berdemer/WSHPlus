'use strict';
function bolumAnaliziCtrl($scope, SMSvc, DTOptionsBuilder, DTColumnDefBuilder, analizSvc, personellerSvc) {
	var baC = this;

	baC.year = (new Date().getFullYear()).toString();
	baC.sonuclar = [{ id: 'hepsi', adi: 'Tümü' }, { id: 'Isbasi', adi: 'İş Başı' }, { id: 'Sevk', adi: 'Sevk' }, { id: 'Istirahat', adi: 'İstirahat' }];
	baC.sonuc = baC.sonuclar[0];
	personellerSvc.MoviesIds.BolumId = 0;
	personellerSvc.MoviesIds.SirketId = 0;
	personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
	personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
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
		raporlar($scope.SirketBasligi.id, 'Ikbilgisi', baC.sonuc.id, baC.year);
		if (angular.isUndefined(newValue)) {
			$scope.SirketBasligi = {
				id: 0,
				name: 'Şirketi Seçiniz.!'
			};
		}
		baC.sirketAdi = $scope.SirketBasligi.id === 0 ? '' : $scope.SirketBasligi.name;
	});
	$scope.dtOptions = DTOptionsBuilder.newOptions()
	 .withLanguageSource('/views/Personel/Controller/Turkish.json')
	 .withDOM('<"html5buttons"B>lTfgitp')
	 .withButtons([
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'İş Kazaları Değerlendirme Raporu', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'İş Kazaları Değerlendirme Raporu', titleAttr: 'PDF' },
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
	 .withOption('lengthMenu', [20, 30, 50, 100])
	 .withOption('order', [1, 'asc'])
	 .withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefs = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	baC.toplam = function (ay) {
		return ay.Ocak + ay.Subat + ay.Mart + ay.Nisan + ay.Mayis + ay.Haziran + ay.Temmuz + ay.Agustos + ay.Eylul + ay.Ekim + ay.Kasim + ay.Aralik;
	};

	var raporlar = function (Sirket_Id, muayeneDurumu, muayeneSonucu, year) {
	    analizSvc.BolumlerinIsKazaSayilari(Sirket_Id, muayeneDurumu, muayeneSonucu, year).then(function (resp) {
	        baC.aylar = resp;
	    });
	};

	$scope.$watch(function () { return baC.year; }, function (newValue, oldValue) {
	    if (newValue !== oldValue) {
	        raporlar($scope.SirketBasligi.id, 'Ikbilgisi', baC.sonuc.id, newValue);
	    }
	});

	$scope.$watch(function () { return baC.sonuc; }, function (newValue, oldValue) {
	    if (newValue !== oldValue) {
	        raporlar($scope.SirketBasligi.id, 'Ikbilgisi', newValue.id, baC.year);
	    }
	});

	baC.lineOptions = {
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

	$scope.$watch(function () { return baC.aylar; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			var labels = ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'];
			var datasets = [];
			baC.series = [];
			angular.forEach(newValue, function (x) {
				var gui = generateUUID();
				var c1 = Math.floor(Math.random() * 256);
				var c2 = Math.floor(Math.random() * 256);
				var c3 = Math.floor(Math.random() * 256);
				baC.series.push(x.bolumAdi);
				datasets.push({
					gui: gui,
					label: x.bolumAdi,
					fillColor: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",0.5)",
					strokeColor: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",1)",
					pointColor: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",1)",
					pointStrokeColor: "#fff",
					pointHighlightFill: "#fff",
					pointHighlightStroke: "rgba(" + c1.toString() + "," + c2.toString() + "," + c3.toString() + ",1)",
					data: [x.Ocak, x.Subat, x.Mart, x.Nisan, x.Mayis, x.Haziran, x.Temmuz, x.Agustos, x.Eylul, x.Ekim, x.Kasim, x.Aralik]
				});
			});
			baC.datasets = datasets;
			baC.lineData = {
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

	baC.lineAddtem = function (gui, i) {
		if (gui) {
			baC.lineData.datasets.push(i);
		} else {
			var index = baC.lineData.datasets.indexOf(i);
			baC.lineData.datasets.splice(index, 1);
		}
	};
}

bolumAnaliziCtrl.$inject = ['$scope', 'SMSvc', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'analizSvc', 'personellerSvc'];

angular
	.module('inspinia')
	.controller('bolumAnaliziCtrl ', bolumAnaliziCtrl);