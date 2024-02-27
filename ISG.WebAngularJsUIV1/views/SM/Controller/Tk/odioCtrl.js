'use strict';

function odioCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var OdC = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	OdC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	OdC.model = {
	};

	OdC.options = {};

	OdC.fields = [
		{
			className: 'row col-sm-12',
			fieldGroup: [
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag250',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '250 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1  kirmizi',
							type: 'input',
							key: 'Sag500',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '500 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag1000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '1000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag2000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '2000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag3000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '3000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag4000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '4000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag5000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '5000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag6000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '6000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag7000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '7000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-1 kirmizi',
							type: 'input',
							key: 'Sag8000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '8000 mHz',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-2'
						}
			]
		},
		{
			className: 'row col-sm-12',
			fieldGroup: [
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol250',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '250 mHz',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol500',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '500 mHz',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol1000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '1000 mHz',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol2000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '2000 mHz',
								placeholder: 'Sol.'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol3000',
							templateOptions: {
								type: 'number',
								min: 0,
								required: true,
								label: '3000 mHz',
								placeholder: 'Sol.'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol4000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '4000 mHz',
								required: true,
								placeholder: 'Sol.'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol5000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '5000',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol6000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '6000 mHz',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol7000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '7000 mHz',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-1 mavi',
							type: 'input',
							key: 'Sol8000',
							templateOptions: {
								type: 'number',
								min: 0,
								label: '8000 mHz',
								placeholder: 'Sol'
							}
						},
						{
							className: 'col-sm-2'
						}
			]
		},
		{
			className: 'row col-sm-12',
			fieldGroup: [
						{
							className: 'col-sm-6',
							type: 'textarea',
							key: 'Sonuc',
							templateOptions: {
								label: 'Sonuç',
								placeholder: ''
							}
						},
						{
							className: 'col-sm-2 kirmizi',
							type: 'input',
							key: 'SagOrtalama',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'Sağ Ortalama',
								placeholder: 'Sağ'
							}
						},
						{
							className: 'col-sm-2 mavi',
							type: 'input',
							key: 'SolOrtalama',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'Sol Ortalama',
								placeholder: 'Sol K.'
							}
						},
						{
							className: 'col-sm-2'
						}
			]
		}
	];

	OdC.originalFields = angular.copy(OdC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.Odiolar = TkSvc.Tk.TkBilgi.data.Odiolar;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.Odiolar = s.data.Odiolar;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsOdC = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'Odio Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'Odio Listesi', titleAttr: 'PDF' },
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
	 .withOption('order', [1, 'desc'])
	 .withOption('lengthMenu', [3, 10, 20, 50, 100])
	 .withOption('rowCallback', rowCallback)
	 .withOption('responsive', true);

	$scope.dtColumnDefsOdC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	OdC.onSubmit = function () {
		OdC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(OdC.model.Odio_Id) || OdC.model.Odio_Id === null) {
			if (TkSvc.Tk.SaglikBirimi_Id !== null) {
				OdC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
				TkSvc.AddPrsOdio(OdC.model, TkSvc.Tk.SaglikBirimi_Id, OdC.protokolGirisi.value).then(function (response) {
					TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
						TkSvc.Tk.TkBilgi = s;
						$scope.Odiolar = s.data.Odiolar;
					});
					$scope.yeniOdC();
				});
			} else {
				notify({
					message: 'Sağlık Birimini Girmeden Kayıt Yapamazsınız.',
					classes: 'alert-danger',
					templateUrl: $scope.inspiniaTemplate,
					duration: 2000,
					position: 'right'
				});
			}
		}
		else {
			TkSvc.UpdatePrsOdio(OdC.model.Odio_Id, OdC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.Odiolar = s.data.Odiolar;
				});
				$scope.yeniOdC();
			}).catch(function (e) {
			    $scope.message = "Hata Kontrol Edin! " + e;
			    startTimer();
			});
		}
	};
	var startTimer = function () {
	    var timer = $timeout(function () {
	        $timeout.cancel(timer);
	        $scope.message = "";
	    }, 4000);
	};
	function rowCallback(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
		$('td', nRow).unbind('click');
		$('td', nRow).bind('click', function () {
			$scope.$apply(function () {
				$scope.isCollapsed = false;
				var rs = $filter('filter')($scope.Odiolar, {
					Odio_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru === null ? '' : rs.MuayeneTuru.trim();
				OdC.model = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'odio', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#odio").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniOdC = function () {
		OdC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silOdC = function () {
		TkSvc.DeletePrsOdio(OdC.model.Odio_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.Odiolar = s.data.Odiolar;
			});
			$scope.yeniOdC();
		}).catch(function (e) {
		    $scope.message = "Hata Kontrol Edin! " + e;
		    startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/odio/' + OdC.model.Odio_Id,
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

	$scope.$watch(function () { return OdC.model.Odio_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue !== null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

	$scope.Vurgusu = function (deger, bicim) {
	    return deger === null ? '----' : deger + bicim;
	};

	OdC.lineOptions = {
		scaleShowGridLines: true,
		scaleGridLineColor: "rgba(0,0,0,.05)",
		scaleGridLineWidth: 1,
		scaleOverride: true,
		scaleSteps: 10,
		scaleStepWidth: 10,
		scaleStartValue: 0,
		scaleLabel:"<%=value+' dB'%>",
		bezierCurve: true,
		bezierCurveTension: 0.4,
		pointDot: true,
		pointDotRadius: 4,
		pointDotStrokeWidth: 1,
		pointHitDetectionRadius: 20,
		datasetStroke: false,
		datasetStrokeWidth: 2,
		datasetFill: false,
		reverse: true
	};

	function nullYap(x) {
		return x === 0||angular.isUndefined(x) ? null : x;
	}

	$scope.$watch('[OdC.model]', function (newValue, oldValue) {
		if (!angular.isUndefined(newValue[0]) || newValue[0] !== null) {
			OdC.model.Sonuc = "";
			$scope.heightx = newValue[0].length > 0 ? 200 : 150;
			OdC.lineData = {
				labels:['0,25kHz','0,5kHz','1kHz','2kHz','3kHz','4kHz','5kHz','6kHz','7kHz','8kHz'],
				datasets: [
					{
						label: "Sağ Kulak",
						fillColor: "rgba(255,20,100,0.5)",
						strokeColor: "rgba(255,20,100,1)",
						pointColor: "rgba(255,20,100,1)",
						pointStrokeColor: "#fff",
						pointHighlightFill: "#fff",
						pointHighlightStroke: "rgba(255,20,100,1)",
						data:[nullYap(newValue[0].Sag250),nullYap(newValue[0].Sag500),nullYap(newValue[0].Sag1000),nullYap(newValue[0].Sag2000),nullYap(newValue[0].Sag3000),
						nullYap(newValue[0].Sag4000),nullYap(newValue[0].Sag5000),nullYap(newValue[0].Sag6000),nullYap(newValue[0].Sag7000),nullYap(newValue[0].Sag8000)]
					},
					{
						label: "Sol Kulak",
						fillColor: "rgba(0,88,255,0.5)",
						strokeColor: "rgba(0,88,255,0.7)",
						pointColor: "rgba(0,88,255,1)",
						pointStrokeColor: "#fff",
						pointHighlightFill: "#fff",
						pointHighlightStroke: "rgba(0,88,255,1)",
						data: [nullYap(newValue[0].Sol250), nullYap(newValue[0].Sol500), nullYap(newValue[0].Sol1000), nullYap(newValue[0].Sol2000), nullYap(newValue[0].Sol3000),
						nullYap(newValue[0].Sol4000), nullYap(newValue[0].Sol5000), nullYap(newValue[0].Sol6000), nullYap(newValue[0].Sol7000), nullYap(newValue[0].Sol8000) ]
					}
				]

			};
			if (!angular.isUndefined(newValue[0].Sag500) && !angular.isUndefined(newValue[0].Sag1000) && !angular.isUndefined(newValue[0].Sag2000))
			{OdC.model.SagOrtalama = Math.round((newValue[0].Sag500 + newValue[0].Sag1000 + newValue[0].Sag2000) / 3);}
			if (!angular.isUndefined(newValue[0].Sol500) && !angular.isUndefined(newValue[0].Sol1000) && !angular.isUndefined(newValue[0].Sol2000))
			{ OdC.model.SolOrtalama = Math.round((newValue[0].Sol500 + newValue[0].Sol1000 + newValue[0].Sol2000) / 3); }
			var sagSonuc = ""; var solSonuc = "";
			switch (true) {
				case OdC.model.SagOrtalama < 26:
					sagSonuc = "";
					break;
				case OdC.model.SagOrtalama > 25 &&  OdC.model.SagOrtalama < 35:
					sagSonuc = "Sağ kulakta Hafif Derece İşitme Kaybı Olabilir. ";
					break;
				case OdC.model.SagOrtalama > 34 &&  OdC.model.SagOrtalama < 45:
					sagSonuc = "Sağ kulakta Orta Derece İşitme Kaybı Olabilir. ";
					break;
				case OdC.model.SagOrtalama > 44:
					sagSonuc = "Sağ kulakta Ağır Derece İşitme Kaybı Olabilir. ";
					break;
			}
			switch (true) {
				case OdC.model.SolOrtalama < 26:
					solSonuc = "";
					break;
				case OdC.model.SolOrtalama > 25 && OdC.model.SolOrtalama < 35:
					solSonuc = "Sol kulakta Hafif Derece İşitme Kaybı Olabilir. ";
					break;
				case OdC.model.SolOrtalama > 34 && OdC.model.SolOrtalama < 45:
					solSonuc = "Sol kulakta Orta Derece İşitme Kaybı Olabilir. ";
					break;
				case OdC.model.SolOrtalama > 44:
					solSonuc = "Sol kulakta Ağır Derece İşitme Kaybı Olabilir. ";
					break;
			}
			OdC.model.Sonuc = sagSonuc + solSonuc;

		}
	},true);
}
odioCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('odioCtrl', odioCtrl);