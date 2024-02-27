'use strict';

function icRaporCtrl($scope, $stateParams, personellerSvc, DTOptionsBuilder, DTColumnDefBuilder, $filter) {
	var irc = this;
	irc.model = {
		IcRapor_Id: undefined, MuayeneTuru: undefined, Tani: undefined, RaporTuru: undefined, BaslangicTarihi: undefined, BitisTarihi: undefined, SureGun: undefined, Doktor_Adi: undefined,
		Personel_Id: undefined, Revir_Id: undefined, Sirket_Id: undefined, Bolum_Id: undefined, PoliklinikMuayene_Id: undefined
	};

	irc.options = {};
	var drLar = [];

	personellerSvc.GetIsgUser("İş Yeri Hekimi").then(function (response) {
		angular.forEach(response, function (value) {
		    drLar.push({ name: value.adi.trim() + ' ' + value.soyadi.trim(), value: value.adi.trim() + ' ' + value.soyadi.trim() });
		});
		personellerSvc.SD.hekimler = drLar;
		irc.fields = [
				{
					className: 'row col-sm-12',
					fieldGroup: [
								{
									className: 'col-sm-3',
									key: 'MuayeneTuru',
									type: 'ui-select-single',
									templateOptions: {
										optionsAttr: 'bs-options',
										ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
										label: 'Muayene Turu',
										required: true,
										valueProp: 'value',
										labelProp: 'name',
										placeholder: 'Seviye Seçiniz',
										options: personellerSvc.SD.muayeneNedeni
									}
								},
								{
									className: 'col-sm-3',
									key: 'RaporTuru',
									type: 'ui-select-single',
									templateOptions: {
										label: 'Rapor Türü',
										optionsAttr: 'bs-options',
										ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
										valueProp: 'value',
										labelProp: 'name',
										options: [{ name: 'Hastalık', value: 'Hastalık' }, { name: 'Terlik', value: 'Terlik' }, { name: 'İş Kazası', value: 'İş Kazası' }]
									}
								},
								{
									className: 'col-sm-6',
									type: 'input',
									key: 'Tani',
									templateOptions: {
										type: 'input',
										label: 'Tanı'
									}
								}
					]
				},
				{
					className: 'row col-sm-12',
					fieldGroup: [
					 {
						 className: 'col-sm-4',
						 key: 'Doktor_Adi',
						 type: 'ui-select-single',
						 templateOptions: {
							 optionsAttr: 'bs-options',
							 ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
							 label: 'İşYeri Hekimi Ad ve Soyadı',
							 required: true,
							 valueProp: 'value',
							 labelProp: 'name',
							 options: drLar
						 }
					 },
					 {
						 className: 'col-sm-3',
						 key: 'BaslangicTarihi',
						 type: 'datepicker',
						 templateOptions: {
							 label: 'Başlama Tarihi',
							 type: 'text',
							 required: true,
							 datepickerPopup: 'dd.MMMM.yyyy',
							 datepickerOptions: {
								 format: 'dd.MM.yyyy'
							 }
						 }
					 },
					 {
						 className: 'col-sm-3',
						 key: 'BitisTarihi',
						 type: 'datepicker',
						 templateOptions: {
							 label: 'Bitiş Tarihi',
							 type: 'text',
							 datepickerPopup: 'dd.MMMM.yyyy',
							 datepickerOptions: {
								 format: 'dd.MM.yyyy'
							 }
						 }
					 },
					 {
						 className: 'col-sm-2',
						 type: 'input',
						 key: 'SureGun',
						 templateOptions: {
							 type: 'number',
							 min:0,
							 label: 'Gün Sayısı'
						 }
					 }
					]
				}
		];
	});

	irc.originalFields = angular.copy(irc.fields);

	if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
		$scope.IcRaporlari = personellerSvc.MoviesIds.personel.data.IcRaporlari;
	} else {
		personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
			personellerSvc.MoviesIds.personel = s;
			$scope.IcRaporlari = s.data.IcRaporlari;
		});
	}
	$scope.isCollapsed = true;
	$scope.dtOptionsirc = DTOptionsBuilder.newOptions()
	 .withLanguageSource('/views/Personel/Controller/Turkish.json')
	 .withDOM('<"html5buttons"B>lTfgitp')
	 .withButtons([
		 {
			 text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
			 className: 'addButton', action: function (e, dt, node, config) {
				 $scope.$apply(function () {
					 $scope.isCollapsed = !$scope.isCollapsed;
				 });
			 }
		 },
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Ic Raporlari Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Ic Raporlari Listesi', titleAttr: 'PDF' },
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
	 .withPaginationType('full_numbers')
	 .withSelect(true)
	 .withOption('order', [1, 'desc'])
	 .withOption('rowCallback', rowCallback)
	 .withOption('responsive', true);

	$scope.dtColumnDefsirc = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.IcRaporlari, {
					IcRapor_Id: aData[0]
				})[0];
				irc.model = {
					IcRapor_Id: aData[0], MuayeneTuru: aData[2], Tani: aData[4], BaslangicTarihi: new Date(rs.BaslangicTarihi),
					BitisTarihi: new Date(rs.BitisTarihi), SureGun: aData[7], Doktor_Adi: aData[8],
					Personel_Id: rs.Personel_Id, Revir_Id: rs.Revir_Id, Sirket_Id: rs.Sirket_Id, Bolum_Id: rs.Bolum_Id,
					RaporTuru: aData[3], PoliklinikMuayene_Id: rs.PoliklinikMuayene_Id
				};
			});
		});
		return nRow;
	}

	irc.onSubmit = function () {
	    irc.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
	    irc.model.Sirket_Id = personellerSvc.MoviesIds.SirketId;
	    irc.model.Bolum_Id = personellerSvc.MoviesIds.BolumId;
	    if (angular.isUndefined(irc.model.IcRapor_Id) || irc.model.IcRapor_Id === null) {
	        personellerSvc.AddPrsIcRapor(irc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.IcRaporlari = s.data.IcRaporlari;
	            });
	            $scope.yeniirc();
	        });
	    } else {
	        personellerSvc.UpdatePrsIcRapor(irc.model.IcRapor_Id, irc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.IcRaporlari = s.data.IcRaporlari;
	            });
	            $scope.yeniirc();
	        });
	    }
	};

	var deselect = function () {
		var table = $("#icRapor").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};
	$scope.yeniirc = function () {
		irc.model = {}; deselect();
	};

	$scope.silirc = function () {
		personellerSvc.DeletePrsIcRapor(irc.model.IcRapor_Id).then(function (response) {
			personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
				personellerSvc.MoviesIds.personel = s;
				$scope.IcRaporlari = s.data.IcRaporlari;
			});
			$scope.yeniirc();
		});
	};

	function gunFarki(date1, date2) {
		var gun_toplami = 1000 * 60 * 60 * 24;
		var date1_as = date1.getTime();
		var date2_as = date2.getTime();
		var farki = Math.abs(date1_as - date2_as);
		return Math.round(farki / gun_toplami);
	}

	$scope.$watchCollection('[irc.model.BaslangicTarihi,irc.model.BitisTarihi]', function (newValues) {
		if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1])) {
			irc.model.SureGun = gunFarki(newValues[1], newValues[0]);
		}
	});
}

icRaporCtrl.$inject = ['$scope', '$stateParams', 'personellerSvc',
	'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter'];

angular
	.module('inspinia')
	.controller('icRaporCtrl', icRaporCtrl);