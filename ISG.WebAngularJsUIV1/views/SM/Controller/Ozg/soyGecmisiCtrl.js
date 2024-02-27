'use strict';

function soyGecmisiCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, OgSvc, SMSvc, $q, $window) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	var Sgc = this;

	$scope.files = [];

	$scope.dosyaList = false;

	Sgc.HastalikTanimiList = function (value) {
		var deferred = $q.defer();
		SMSvc.HastalikAra(value).then(function (response) {
			deferred.resolve(response);
		});
		return deferred.promise;
	};

	Sgc.model = {

	};

	Sgc.AkrabalikDurumi = [{ name: 'Anne', value: 'Anne' }, { name: 'Baba', value: 'Baba' }, { name: 'Kız Kardeş', value: 'Kız Kardeş' }, { name: 'Erkek Kardeş', value: 'Erkek Kardeş' },
						 { name: 'Baba II. Kuşak Akraba', value: 'Baba II. Kuşak Akraba' }, { name: 'Anne II. Kuşak Akraba', value: 'Anne II. Kuşak Akraba' }, { name: 'Baba III. Kuşak Akraba', value: 'Baba III. Kuşak Akraba' },
						  { name: 'Anne III. Kuşak Akraba', value: 'Anne III. Kuşak Akraba' }
	];
	Sgc.HastalikAdi = [{ name: 'Alzheimer hastalığı', value: 'Alzheimer hastalığı' }, { name: 'Astım ve alerjiler', value: 'Astım ve alerjiler' },
	{ name: 'Doğum anomalileri', value: 'Doğum anomalileri' }, { name: 'Görme kaybı, körlük', value: 'Görme kaybı, körlük' },
	{ name: 'Kanserler (meme, yumurtalık, kalın bağırsak, prostat gibi)', value: 'Kanserler (meme, yumurtalık, kalın bağırsak, prostat gibi)' },
	{ name: 'Genç yaşta işitme kaybı', value: 'Genç yaşta işitme kaybı' }, { name: 'Gelişme, zekâ geriliği', value: 'Gelişme, zekâ geriliği' }, { name: 'Şeker hastalığı', value: 'Şeker hastalığı' },
	{ name: 'Kalp hastalığı', value: 'Kalp hastalığı' }, { name: 'Yüksek tansiyon', value: 'Yüksek tansiyon' }, { name: 'Yüksek kolesterol', value: 'Yüksek kolesterol' }, { name: 'Ameliyatlar', value: 'Ameliyatlar' },
	{ name: 'Ruh hastalıkları (Depresyon, Şizofreni)', value: 'Ruh hastalıkları (Depresyon, Şizofreni)' }, { name: 'Obezite', value: 'Obezite' },
	{ name: 'Felç', value: 'Felç' }, { name: 'Madde bağımlılığı (Alkol, sigara)', value: 'Madde bağımlılığı (Alkol, sigara)' }, { name: 'Gebelikte sorunlar', value: 'Gebelikte sorunlar' }];

	$scope.evetHayir = function (value) {
		return value === true ? 'Evet' : 'Hayır';
	};

	Sgc.options = {};

	Sgc.fields = [
					{
						className: 'row col-sm-12',
						fieldGroup: [
										{
											className: 'col-sm-3',
											key: 'AkrabalikDurumi',
											type: 'ui-select-single',
											templateOptions: {
												label: 'Akrabalık',
												optionsAttr: 'bs-options',
												ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
												valueProp: 'value',
												labelProp: 'name',
												required: true,
												options: Sgc.AkrabalikDurumi
											}
										},
										{
											className: 'col-sm-3',
											key: 'HastalikAdi',
											type: 'ui-select-single',
											templateOptions: {
												label: 'Hastalık Grubu',
												optionsAttr: 'bs-options',
												ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
												valueProp: 'value',
												labelProp: 'name',
												required: true,
												options: Sgc.HastalikAdi
											}
										},
										{
											className: 'col-sm-4',
											type: 'typhead',
											key: 'ICD10',
											templateOptions: {
												label: 'ICD10',
												placeholder: 'ICD10 sınıflamasına göre',
												required: true,
												options: [],
												onKeyup: function ($viewValue, $scope) {
													if (typeof $viewValue !== 'undefined') {
														return $scope.templateOptions.options = Sgc.HastalikTanimiList($viewValue);
													}
												}
											}
										},
										{
											className: 'col-sm-2',
											key: 'HastalikSuAnAktifmi',
											type: 'select',
											defaultValue: false,
											templateOptions: {
												label: 'Hastalık Şu An Aktifmi?',
												options: [{ name: 'Hayır', value: false }, { name: 'Evet', value: true }]
											}
										}
						]
					},
					{
						className: 'row col-sm-12',
						fieldGroup: [
								{
									className: 'col-sm-2',
									type: 'input',
									key: 'AkrabaninYasi',
									templateOptions: {
										type: 'number',
										min: 0,
										label: 'Yaşı',
										placeholder: 'Akrabanın Yaşı'
									}
								},
								{
									className: 'col-sm-2',
									type: 'input',
									key: 'AkrabaninHastaOlduguYas',
									templateOptions: {
										type: 'number',
										min: 0,
										label: 'Hastalık Yaşı',
										placeholder: 'Hastalık Başlama Yaşı'
									}
								},
								{
									className: 'col-sm-3',
									type: 'input',
									key: 'OlumNedeni',
									templateOptions: {
										type: 'input',
										label: 'Ölum Nedeni',
										placeholder: 'Varsa Ölüm Nedeni'
									}
								},
								{
									className: 'col-sm-2',
									type: 'input',
									key: 'OlumYasi',
									templateOptions: {
										type: 'number',
										min: 0,
										label: 'Ölüm Yaşı',
										placeholder: 'Varsa Ölüm Yaşı'
									}
								},
								{
									className: 'col-sm-3',
									type: 'textarea',
									key: 'Aciklama',
									templateOptions: {
										label: 'Açıklama',
										rows: 2
									}
								}
						]
					}
	];

	Sgc.originalFields = angular.copy(Sgc.fields);

	if ($stateParams.id === OgSvc.Oz.guidId) {
		$scope.SoyGecmisleri = OgSvc.Oz.OzBilgi.data.SoyGecmisleri;
	}
	else {
		OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
			OgSvc.Oz.OzBilgi = s;
			$scope.SoyGecmisleri = s.data.SoyGecmisleri;
			OgSvc.Oz.guidId = $stateParams.id;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsSgc = DTOptionsBuilder.newOptions()
	 .withLanguageSource('/views/Personel/Controller/Turkish.json')
	 .withDOM('<"html5buttons"B>lTfgitp')
	 .withButtons([
		 {
			 text: '<i class="fa fa-plus"></i>', titleAttr: 'Detay Aç', key: '1',
			 className: 'addButton', action: function (e, dt, node, config) {
				 $scope.$apply(function () {
				     $scope.isCollapsed = !$scope.isCollapsed;
				     $scope.dosyaList = !$scope.dosyaList;
				 });
			 }
		 },
		 { extend: 'copy', text: '<i class="fa fa-files-o"></i>', titleAttr: 'Kopyala' },
		 { extend: 'csv', text: '<i class="fa fa-file-text-o"></i>', titleAttr: 'Excel 2005' },
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'SoyGecmisleriListesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'SoyGecmisleriListesi', titleAttr: 'PDF' },
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
	 .withOption('lengthMenu', [3, 10, 20, 50, 100])
	 .withOption('rowCallback', rowCallback)
		.withOption('responsive', window.innerWidth < 1500 ? true : false);

	$scope.dtColumnDefsSgc = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	Sgc.onSubmit = function () {
		Sgc.model.Personel_Id = OgSvc.Oz.OzBilgi.data.Personel_Id;
		if (angular.isUndefined(Sgc.model.SoyGecmisi_Id) || Sgc.model.SoyGecmisi_Id == null) {
			OgSvc.AddPrsSoyGecmisi(Sgc.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.SoyGecmisleri = s.data.SoyGecmisleri;
				});
				$scope.yeniSgc();
			});
		}
		else {
			OgSvc.UpdatePrsSoyGecmisi(Sgc.model.SoyGecmisi_Id, Sgc.model).then(function (response) {
				OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
					OgSvc.Oz.OzBilgi = s;
					$scope.SoyGecmisleri = s.data.SoyGecmisleri;
				});
				$scope.yeniSgc();
			});
		}
	};

	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.SoyGecmisleri, {
					SoyGecmisi_Id: aData[0]
				})[0];
				Sgc.model = {
				    SoyGecmisi_Id: rs.SoyGecmisi_Id, Personel_Id: rs.Personel_Id, UserId: rs.UserId,AkrabaninYasi:rs.AkrabaninYasi,
					AkrabalikDurumi: rs.AkrabalikDurumi.trim(), HastalikAdi: rs.HastalikAdi.trim(), AkrabaninHastaOlduguYas: rs.AkrabaninHastaOlduguYas,
					HastalikSuAnAktifmi: rs.HastalikSuAnAktifmi, ICD10: rs.ICD10, OlumNedeni: rs.OlumNedeni, OlumYasi: rs.OlumYasi, Aciklama: rs.Aciklama
				};
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'soy-gecmisi', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}
	var deselect = function () {
		var table = $("#soyGecmisi").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniSgc = function () {
		Sgc.model = {}; $scope.files = [];
		$scope.dosyaList = false;
		deselect();
	};

	$scope.silSgc = function () {
		OgSvc.DeletePrsSoyGecmisi(Sgc.model.SoyGecmisi_Id).then(function (response) {
			OgSvc.GetOzgecmisiPersonel($stateParams.id).then(function (s) {
				OgSvc.Oz.OzBilgi = s;
				$scope.SoyGecmisleri = s.data.SoyGecmisleri;
			});
			$scope.yeniSgc();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/soy-gecmisi/' + Sgc.model.SoyGecmisi_Id,
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
			if (response.data == 1) { $scope.files.splice(index, 1); }
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

	$scope.$watch(function () { return Sgc.model.SoyGecmisi_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue != null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

}

soyGecmisiCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'OgSvc', 'SMSvc', '$q', '$window'];

angular
	.module('inspinia')
	.controller('soyGecmisiCtrl', soyGecmisiCtrl);