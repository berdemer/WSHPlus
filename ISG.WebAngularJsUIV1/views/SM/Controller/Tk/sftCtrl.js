'use strict';

function sftCtrl($scope, DTOptionsBuilder, DTColumnDefBuilder, $filter, Upload, $stateParams,
	 ngAuthSettings, uploadService, authService, TkSvc, SMSvc, $q, $window, $timeout, notify) {

	var serviceBase = ngAuthSettings.apiServiceBaseUri;

	TkSvc.Tk.MuayeneTuru = { muayene: "Revir İşlemleri" };

	var SftC = this;

	$scope.opsiyonlar = [{ adi: 'Hayır', value: false }, { adi: 'Evet', value: true }];

	SftC.protokolGirisi = $scope.opsiyonlar[0];

	$scope.files = [];

	$scope.dosyaList = false;

	SftC.model = {
	};

	SftC.options = {};

	SftC.fields = [
		{
		className: 'row col-sm-12',
		fieldGroup: [
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'FVC',
						templateOptions: {
							type: 'number',
							min: 0,
							required: true,
							label: 'FVC',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'FEV1',
						templateOptions: {
							type: 'number',
							required: true,
							min: 0,
							label: 'FEV1',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'Fev1Fvc',
						templateOptions: {
							type: 'number',
							min: 0,
							required: true,
							label: 'FEV1/FVC',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-2',
						type: 'input',
						key: 'PEF',
						templateOptions: {
							type: 'number',
							min: 0,
							required: true,
							label: 'PEF',
							placeholder: ''
						}
					},
					{
						className: 'col-sm-4',
						type: 'textarea',
						key: 'Sonuc',
						templateOptions: {
							label: 'Sonuç',
							placeholder: ''
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
							key: 'VC',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'VC',
								placeholder: ''
							}
						},
						{
							className: 'col-sm-2',
							type: 'input',
							key: 'RV',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'RV',
								placeholder: ''
							}
						},
						{
							className: 'col-sm-2',
							type: 'input',
							key: 'TLC',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'TLC',
								placeholder: ''
							}
						},
						{
							className: 'col-sm-2',
							type: 'input',
							key: 'Fev2575',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'FEF25-75%',
								placeholder: ''
							}
						},
						{
							className: 'col-sm-2',
							type: 'input',
							key: 'FEV50',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'FEV%50',
								placeholder: ''
							}
						},
						{
							className: 'col-sm-2',
							type: 'input',
							key: 'MVV',
							templateOptions: {
								type: 'number',
								min: 0,
								label: 'MVV',
								placeholder: ''
							}
						}
			]
		}
	];

	SftC.originalFields = angular.copy(SftC.fields);

	if ($stateParams.id === TkSvc.Tk.guidId) {
		$scope.SFTleri = TkSvc.Tk.TkBilgi.data.SFTleri;
	}
	else {
		TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
			TkSvc.Tk.TkBilgi = s;
			TkSvc.Tk.guidId = $stateParams.id;
			$scope.SFTleri = s.data.SFTleri;
			$scope.fileImgPath = s.img.length > 0 ? ngAuthSettings.apiServiceBaseUri + s.img[0].LocalFilePath : ngAuthSettings.apiServiceBaseUri + 'uploads/';
			$scope.img = s.img.length > 0 ? s.img[0].GenericName : "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
			$scope.Ozg = s;
			$scope.AdSoyad = s.data.Adi + ' ' + s.data.Soyadi;
		});
	}

	$scope.isCollapsed = true;

	$scope.dtOptionsSftC = DTOptionsBuilder.newOptions()
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
		 { extend: 'excel', text: '<i class="fa fa-file-excel-o"></i>', title: 'SFT Listesi', titleAttr: 'Excel 2010' },
		 { extend: 'pdf', text: '<i class="fa fa-file-pdf-o"></i>', title: 'SFT Listesi', titleAttr: 'PDF' },
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

	$scope.dtColumnDefsSftC = [// 0. kolonu gizlendi.
	 DTColumnDefBuilder.newColumnDef(0).notVisible()
	];

	$scope.inspiniaTemplate = 'views/common/notify.html';

	SftC.onSubmit = function () {
		SftC.model.Personel_Id = TkSvc.Tk.TkBilgi.data.Personel_Id;
		if (angular.isUndefined(SftC.model.Sft_Id) || SftC.model.Sft_Id == null) {
		    if (TkSvc.Tk.SaglikBirimi_Id !== null) {
		        SftC.model.MuayeneTuru = TkSvc.Tk.MuayeneTuru.muayene;
		        TkSvc.AddPrsSFT(SftC.model, TkSvc.Tk.SaglikBirimi_Id, SftC.protokolGirisi.value).then(function (response) {
		            TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
		                TkSvc.Tk.TkBilgi = s;
		                $scope.SFTleri = s.data.SFTleri;
		            });
		            $scope.yeniSftC();
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
			TkSvc.UpdatePrsSFT(SftC.model.Sft_Id, SftC.model).then(function (response) {
				TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
					TkSvc.Tk.TkBilgi = s;
					$scope.SFTleri = s.data.SFTleri;
				});
				$scope.yeniSftC();
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
				var rs = $filter('filter')($scope.SFTleri, {
					Sft_Id: aData[0]
				})[0];
				rs.MuayeneTuru = angular.isUndefined(rs.MuayeneTuru) || rs.MuayeneTuru == null ? '' : rs.MuayeneTuru.trim();
				SftC.model = rs;
				$scope.files = [];
				$scope.dosyaList = false;
				uploadService.GetFileId($stateParams.id, 'sft', aData[0]).then(function (response) {
					$scope.files = response;
					$scope.dosyaList = true;
				});
			});
			return nRow;
		});
	}

	var deselect = function () {
		var table = $("#sft").DataTable();
		table
			.rows('.selected')
			.nodes()
			.to$()
			.removeClass('selected');

	};

	$scope.yeniSftC = function () {
		SftC.model = {};
		$scope.files = [];
		$scope.dosyaList = false;
		deselect();
		$scope.BKIyorum = "";
		$scope.BKOyorum = "";
	};

	$scope.silSftC = function () {
		TkSvc.DeletePrsSFT(SftC.model.Sft_Id).then(function (response) {
			TkSvc.GetTkPersonel($stateParams.id).then(function (s) {
				TkSvc.Tk.TkBilgi = s;
				$scope.SFTleri = s.data.SFTleri;
			});
			$scope.yeniSftC();
		}).catch(function (e) {
		    $scope.message = "Hata Kontrol Edin! " + e;
		    startTimer();
		});
	};

	$scope.uploadFiles = function (dataUrl) {
		if (dataUrl && dataUrl.length) {
			$scope.dosyaList = true;
			Upload.upload({
				url: serviceBase + ngAuthSettings.apiUploadService + $stateParams.id + '/sft/' + SftC.model.Sft_Id,
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

	$scope.$watch(function () { return SftC.model.Sft_Id; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue != null) {
			$scope.fileButtonShow = true;
		} else { $scope.fileButtonShow = false; }
	});

	$scope.Vurgusu = function (deger, bicim) {
	    return deger == null ? '----' : deger + bicim;
	};

	SftC.barOptions = {
		scaleBeginAtZero: true,
		scaleShowGridLines: true,
		scaleGridLineColor: "rgba(0,0,0,.05)",
		scaleGridLineWidth: 1,
		barShowStroke: true,
		barStrokeWidth: 2,
		barValueSpacing: 5,
		barDatasetSpacing: 1
	};

	$scope.$watch(function () { return $scope.SFTleri; }, function (newValue, oldValue) {
		if (!angular.isUndefined(newValue) || newValue != null) {
			var label = []; var fvc = []; var fev = []; var pef = [];
			angular.forEach(newValue, function (item) {
				var date = new Date(item.Tarih);
				label.push(date.toLocaleDateString());
				fvc.push(item.FVC);
				fev.push(item.FEV1);
				pef.push(item.PEF);
			});
			$scope.heightx = newValue.length > 0 ? 200 : 150;
			SftC.barData = {
				labels: label.length > 0 ? label : [''],
				datasets: [
					{
						label: "FVC",
						fillColor: "rgba(220,220,220,0.5)",
						strokeColor: "rgba(220,220,220,0.8)",
						highlightFill: "rgba(220,220,220,0.75)",
						highlightStroke: "rgba(220,220,220,1)",
						data: fvc.length > 0 ? fvc : [1]
					},
					{
						label: "FEV",
						fillColor: "rgba(26,179,148,0.5)",
						strokeColor: "rgba(26,179,148,0.8)",
						highlightFill: "rgba(26,179,148,0.75)",
						highlightStroke: "rgba(26,179,148,1)",
						data: fev.length > 0 ? fev : [1]
					},
					{
						label: "PEF",
						fillColor: "rgba(255,99,132,0.5)",
						strokeColor: "rgba(255,99,132,0.8)",
						highlightFill: "rgba(255,99,132,0.75)",
						highlightStroke: "rgba(255,99,132,1)",
						data: pef.length > 0 ? pef : [1]
					}
				]

			};
		}
	});

	$scope.$watchCollection('[SftC.model.Fev1Fvc,SftC.model.FVC,SftC.model.FEV1]', function (newValues) {
	    if (!angular.isUndefined(newValues[0]) && !angular.isUndefined(newValues[1]) && !angular.isUndefined(newValues[2])) {
	        SftC.model.Sonuc = "Normal olarak değerlendirildi";
	        if (newValues[0] < 80) {
	            if (newValues[1] < 80) {
	                SftC.model.Sonuc = "Kombine tip tıkanklık olabilir.";
	            } else {
	                if (newValues[2] < 80) {SftC.model.Sonuc = "Obstrüktif tip tıkanklık olabilir."; }
	            }
	        } else {
	            if (newValues[1] < 80) {
	                SftC.model.Sonuc = "Restriktif tip tıkanklık olabilir.";
	            } else {
	                SftC.model.Sonuc = "Normal olarak değerlendirildi"; 
	            }
	        }
	        console.log(SftC.model.Sonuc);
	        switch (true) {
	            case newValues[0]>=80:
	                SftC.model.Sonuc = SftC.model.Sonuc;
	                break;
	            case newValues[0]< 80 && newValues[0]> 59:
	                SftC.model.Sonuc = SftC.model.Sonuc+" Hafif Derece Obstrüksiyon.";
	                break;
	            case newValues[0]< 60 && newValues[0]> 39:
	                SftC.model.Sonuc = SftC.model.Sonuc + " Orta Derece Obstrüksiyon.";
	                break;
	            case newValues[0]< 40:
	                SftC.model.Sonuc = SftC.model.Sonuc + " Ağır Derece Obstrüksiyon.";
	                break;
	        }
	        console.log(SftC.model.Sonuc);
	    }
	});


}
sftCtrl.$inject = ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$filter', 'Upload', '$stateParams',
	'ngAuthSettings', 'uploadService', 'authService', 'TkSvc', 'SMSvc', '$q', '$window', '$timeout', 'notify'];

angular
	.module('inspinia')
	.controller('sftCtrl', sftCtrl);