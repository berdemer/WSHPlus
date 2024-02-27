'use strict';
function calismaDurumuUpdateCrtl($scope, $stateParams, personellerSvc, DTOptionsBuilder,
	DTColumnDefBuilder, $filter, Upload, $window, uploadService, ngAuthSettings) {

	var cduc = this;
	cduc.model = {
		Calisma_Durumu_Id: undefined, Sirket: undefined, Sirket_Id: undefined, Bolum: undefined, Bolum_Id: undefined,
		Baslama_Tarihi: undefined, Bitis_Tarihi: undefined, Status: undefined, Aciklama: undefined,
		Personel_Id: undefined, Calisma_Duzeni: undefined, KadroDurumu: undefined, Gorevi: undefined, SicilNo: undefined
	};

	cduc.onSubmit = function () {
	    cduc.model.Personel_Id = personellerSvc.MoviesIds.personel.data.Personel_Id;
	    cduc.model.Sirket = personellerSvc.MoviesIds.SirketAdi;
	    cduc.model.Sirket_Id = personellerSvc.MoviesIds.SirketId;
	    cduc.model.Bolum = personellerSvc.MoviesIds.BolumAdi;
	    cduc.model.Bolum_Id = personellerSvc.MoviesIds.BolumId;

	    if (angular.isUndefined(cduc.model.Calisma_Durumu_Id) || cduc.model.Calisma_Durumu_Id === null) {//yeni Kayıt
	        personellerSvc.AddPrsCalisma_Durumu(cduc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.CalismaDurumlari = s.data.Calisma_Durumu;
	            });
	            $scope.yeniCD();
	        });
	    } else {//düzenle
	        personellerSvc.UpdatePrsCalisma_Durumu(cduc.model.Calisma_Durumu_Id, cduc.model).then(function (response) {
	            personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
	                personellerSvc.MoviesIds.personel = s;
	                $scope.CalismaDurumlari = s.data.Calisma_Durumu;
	            });
	            $scope.yeniCD();
	        });
	    }
	};

	$scope.$watch(function () { return personellerSvc.MoviesIds.SirketId; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			cduc.model.Sirket_Id = newValue;
			cduc.model.Sirket = personellerSvc.MoviesIds.SirketAdi;
		}
		if (newValue > 0) {
			$scope.savedSuccessfully = false;
			$scope.message = "";
		}
	});

	$scope.$watch(function () { return personellerSvc.MoviesIds.BolumId; }, function (newValue, oldValue) {
		if (newValue !== oldValue) {
			cduc.model.Bolum_Id = newValue;
			cduc.model.Bolum = personellerSvc.MoviesIds.BolumAdi;
		}
	});

	cduc.options = {};

	cduc.fields = [
					{
						className: 'row col-sm-12',
						fieldGroup: [
										{
											className: 'col-sm-3',
											key: 'Gorevi',
											type: 'ui-select-single',
							             	defaultValue: 'Personel',
											templateOptions: {
												optionsAttr: 'bs-options',
												ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
												label: 'Görevi',
												required: true,
												valueProp: 'val',
												labelProp: 'val',
												placeholder: 'Görevini Seçiniz',
												options: personellerSvc.SD.gorevler
											}
										},
										{
											className: 'col-sm-3',
											key: 'KadroDurumu',
											type: 'ui-select-single',
											defaultValue: 'İşçi',
											templateOptions: {
												optionsAttr: 'bs-options',
												ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
												label: 'Kadro/Ünvan Durumu',
												required: true,
												valueProp: 'val',
												labelProp: 'val',
												placeholder: 'Kadrosunu Seçiniz',
												options: personellerSvc.SD.unvanlar
											}
										},
										{
											className: 'col-sm-3',
											key: 'Baslama_Tarihi',
											type: 'datepicker',
											defaultValue: new Date(),
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
											key: 'Bitis_Tarihi',
											type: 'datepicker',
											templateOptions: {
												label: 'Bitiş Tarihi',
												type: 'text',
												datepickerPopup: 'dd.MMMM.yyyy',
												datepickerOptions: {
													format: 'dd.MM.yyyy'
												}
											}
										}
						]
					},
					{
						className: 'row col-sm-12',
						fieldGroup: [
							{
								className: 'col-sm-3',
								key: 'Status',
								type: 'ui-select-single',
								defaultValue: true,
								templateOptions: {
									label: 'Durumu',
									optionsAttr: 'bs-options',
									ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
									valueProp: 'value',
									labelProp: 'name',
									options: [{ name: 'Aktif', value: true }, { name: 'Pasif', value: false}]
								}
							},
							{
								className: 'col-sm-3',
								key: 'Calisma_Duzeni',
								type: 'ui-select-single',
								defaultValue: { name: 'Vardiya', value: 'Vardiya' },
								templateOptions: {
									label: 'Çalışma Düzeni',
									optionsAttr: 'bs-options',
									ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
									valueProp: 'value',
									labelProp: 'name',
									options: [{ name: 'Vardiya', value: 'Vardiya' }, { name: 'A VARDİYASI', value: 'A VARDİYASI' }, { name: 'B VARDİYASI', value: 'B VARDİYASI' }, { name: 'C VARDİYASI', value: 'C VARDİYASI' }, { name: 'D VARDİYASI', value: 'D VARDİYASI' }, { name: '08:00-18:00 x 5 gün', value: '08:00-18:00 x 5 gün' }, { name: '08:00-16:00 x 6 gün', value: '08:00-16:00 x 6 gün' },
										{ name: 'GÜNDÜZ', value: 'GÜNDÜZ' }, { name: 'Geçiçi', value: 'Geçiçi' }, { name: 'Gece', value: 'Gece' }, { name: 'JOKER VARDİYASI', value: 'JOKER VARDİYASI' }]
								}
							},
							{
								className: 'col-sm-2',
								type: 'input',
								key: 'SicilNo',
								templateOptions: {
									type: 'input',
									label: 'Sicil No'
								}
							},
							{
								className: 'col-sm-4',
								type: 'textarea',
								key: 'Aciklama',
								templateOptions: {
									label: 'Detay',
									 rows: 2
								}
							}
						]
					}
	];

	cduc.originalFields = angular.copy(cduc.fields);


	if ($stateParams.id === personellerSvc.MoviesIds.guidId) {
		$scope.CalismaDurumlari = personellerSvc.MoviesIds.personel.data.Calisma_Durumu;
	} else {
		personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
			personellerSvc.MoviesIds.personel = s;
			$scope.CalismaDurumlari = s.data.Calisma_Durumu;
		});
	}

	$scope.isCollapsed = true;
 $scope.dtOptionsx = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Calisma Durumlari Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Calisma Durumlari Listesi', titleAttr: 'PDF' },
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
   .withSelect(
		{
			style: 'single',
			blurable: true
		}
   )
   .withOption('order', [1, 'desc'])
   .withOption('rowCallback', rowCallback)
   .withOption('responsive', true);

 $scope.dtColumnDefsx = [// 0. kolonu gizlendi.
  DTColumnDefBuilder.newColumnDef(0).notVisible()
 ];

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
		    $scope.isCollapsed = false;
			$scope.$apply(function () {
				var rs = $filter('filter')($scope.CalismaDurumlari, {
					Calisma_Durumu_Id: aData[0]
				})[0]; 

				cduc.model = {
					Calisma_Durumu_Id: aData[0], Sirket: aData[2], Sirket_Id: rs.Sirket_Id, Bolum: aData[3], Bolum_Id: rs.Bolum_Id,
					Baslama_Tarihi: new Date(rs.Baslama_Tarihi), Bitis_Tarihi: new Date(rs.Bitis_Tarihi), Status: rs.Status, Aciklama: rs.Aciklama,
					Personel_Id: rs.Personel_Id, Calisma_Duzeni: aData[8], KadroDurumu: aData[7], Gorevi: aData[4], SicilNo: rs.SicilNo
				};
				personellerSvc.MoviesIds.SirketId = rs.Sirket_Id;
				personellerSvc.MoviesIds.SirketAdi = aData[2];
				personellerSvc.MoviesIds.BolumId = rs.Bolum_Id;
				personellerSvc.MoviesIds.BolumAdi = aData[3];
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'calisma-durumu', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
		}); 
		return nRow;
	}

	var deselect = function () {
		var table = $("#calismaDurumu").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniCD = function () {
		cduc.model = {};
		$scope.dosyaList = false;
		$scope.files = []; deselect();
	};

	$scope.silCD = function () {
		personellerSvc.DeletePrsCalisma_Durumu(cduc.model.Calisma_Durumu_Id).then(function (response) {
			personellerSvc.GetGuidPersonel($stateParams.id).then(function (s) {
				personellerSvc.MoviesIds.personel = s;
				$scope.CalismaDurumlari = s.data.Calisma_Durumu;
			});
			$scope.yeni();
		});
	};

	$scope.files = [];

	$scope.dosyaList = false;

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
			    url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/calisma-durumu/' + cduc.model.Calisma_Durumu_Id,
				method: 'POST',
				data: {
					file: dataUrl
				}
			}).then(function (response) {
			    angular.forEach(response.data, function (data) {
			        $scope.files.push(data);
			    });
			});
		}
		else {
			$scope.dosyaList = false;
		}
	};

	$scope.deleten = function (file, index) {
		uploadService.DeleteFile(file + '/x').then(function (response) {
			if (response.data === 1) { $scope.files.splice(index, 1); }
		});
	};

	$scope.download = function (f) {
		debugger;
		if (f.GenericName.indexOf('.') < 1) {
			//$scope.RenderFile(genericName);
			var s = serviceBase + ngAuthSettings.apiUploadService + 'down/' + f.GenericName;
			$window.open(s, '_self', '');
		} else {
			window.open(f.LocalFilePath, 'popUpWindow', 'height=1000,width=1200,left=10,top=10,,scrollbars=yes,menubar=no')
		}
	};

	$scope.$watch(function () { return cduc.model.Calisma_Durumu_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

calismaDurumuUpdateCrtl.$inject = ['$scope', '$stateParams', 'personellerSvc',
	'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter',
	'Upload', '$window', 'uploadService', 'ngAuthSettings'];

angular
	.module('inspinia')
	.controller('calismaDurumuUpdateCrtl', calismaDurumuUpdateCrtl);
