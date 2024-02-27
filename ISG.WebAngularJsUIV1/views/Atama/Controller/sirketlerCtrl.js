'use strict';
function sirketlerCrtl($scope, DTOptionsBuilder, Upload, sirketSvc,
	authService, notify, uploadService, ngAuthSettings, $timeout, $filter) {

	var vm = this;
	var GetAllData = function () {
		sirketSvc.GetSirket(true).then(function (data) {
			$scope.data = data;
		});
	};
	GetAllData();
    $scope.active = 0;
	$scope.getRemoteAllData = function (boole) {
		sirketSvc.GetSirket(boole).then(function (data) {
			$scope.data = data;
		});
	};

    $scope.sel = function (m) {
        $scope.active = null;
        $scope.active = m;
    };

	var _sirket = {
		id: '',
		idRef: '',
		sirketAdi: '',
		status: true
	};

	$scope.treeOptions = {
		accept: function (sourceNodeScope, destNodesScope, destIndex) {
			return true;
		},
		dropped: function (e) {
			var nodeData = e.source.nodeScope.$modelValue;//aktif dugumu
			var newNodeData = e.dest.nodesScope.$parent.$modelValue;//yeni ust dugumu
			//console.log(e.source.nodeScope.$parentNodeScope.$modelValue);//eski alt dugumu
			_sirket = {
				id: nodeData.tabloId,
				idRef: newNodeData === undefined ? 0 : newNodeData.tabloId,
				sirketAdi: nodeData.Text,
				status: true
			};
			sirketSvc.UpdateSirket(nodeData.tabloId, _sirket).then(function (response) {
				GetAllData();
			}, function (err) {
				//if (err.field && err.msg) {
				//} else {
				//}
			});
		}
	};

	$scope.saveUser = function (scope) {
		var nodeData = scope.$modelValue;
		_sirket = {
			id: nodeData.tabloId,
			idRef: nodeData.Depth,
			sirketAdi: nodeData.Text,
			status: true
		};
		sirketSvc.UpdateSirket(nodeData.tabloId, _sirket).then(function (response) {
			GetAllData();
		}, function (err) {
			if (err.field && err.msg) {
				// $scope.editableForm.$setError(err.field, err.msg);
			} else {
				//  $scope.editableForm.$setError('Giriş', 'Unknown error!');
			}
		});
	};

	$scope.remove = function (scope) {
		scope.remove();
	};

	$scope.passiveStatus = function (scope) {
		var nodeData = scope.$modelValue;
		_sirket = {
			id: nodeData.tabloId,
			idRef: nodeData.Depth,
			sirketAdi: nodeData.Text,
			status: nodeData.status === true ? false : true
		};
		sirketSvc.UpdateSirket(nodeData.tabloId, _sirket).then(function (response) {
			// $scope.getRemoteAllData()
		}, function (error) { console.log(error); });
	};


	$scope.toggle = function (scope) {
		scope.toggle();
	};

	$scope.moveLastToTheBeginning = function () {
		var a = $scope.data.pop();
		$scope.data.splice(0, 0, a);
	};

	$scope.newSubItem = function (scope) {//yeni kayıt
		var nodeData = scope.$modelValue;
		_sirket = {
			id: '',
			idRef: nodeData.tabloId,
			sirketAdi: nodeData.Text + '/Güncelleyiniz.',
			status: true
		};
		sirketSvc.InsertSirket(_sirket).then(function (response) {
			GetAllData();
		}, function (err) {
			if (err.field && err.msg) {
				// $scope.editableForm.$setError(err.field, err.msg);
			} else {
				// $scope.editableForm.$setError('Giriş', 'Bilinmeyen Hata!');
			}
		});
	};

	$scope.newSubSirket = function () {//yeni kayıt
		_sirket = {
			id: '',
			idRef: 0,
			sirketAdi: 'Yeni Şirket/Güncelleyiniz.',
			status: true
		};
		sirketSvc.InsertSirket(_sirket).then(function (response) {
			GetAllData();
		});
	};

	var getRootNodesScope = function () {
		return angular.element(document.getElementById("tree-root")).scope();
	};

	$scope.collapseAll = function () {
		$scope.$broadcast('angular-ui-tree:collapse-all');
	};

	$scope.expandAll = function () {
		$scope.$broadcast('angular-ui-tree:expand-all');
	};

	/////////Bölüm tanımları\\\\\\\\\\
	$scope.SirketAtama = function (scope) {
		var nodeData = scope.$modelValue;
		$scope.sirket = {
			id: nodeData.tabloId,
			idRef: nodeData.Depth,
			sirketAdi: nodeData.Text,
			status: true
		};
	};

	var _sirketBolumu = {
		id: '',
		idRef: '',
		bolumAdi: '',
		status: true,
		sirket_Id: ''
	};

	$scope.getSirketBolumuAllData = function (boole) {
		sirketSvc.GetSirketBolumu(boole, $scope.sirket.id).then(function (data) {
			$scope.data2 = data;
		});
	};
	// $scope.getSirketBolumuAllData(true);

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
				//if (err.field && err.msg) {
				//} else {
				//}
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
			sirket_Id: $scope.sirket.id
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

    $scope.mailAc = true;

	$scope.passiveBolumStatus = function (scope) {
		var nodeData = scope.$modelValue;
        _sirketBolumu = {
			id: nodeData.tabloId,
			idRef: nodeData.Depth,
			bolumAdi: nodeData.Text,
			status: nodeData.status === true ? false : true,
			sirket_Id: $scope.sirket.id
		};
        sirketSvc.UpdateSirketBolumu(nodeData.tabloId, _sirketBolumu).then(function (response) {
            console.log(response);
        }, function (error) { console.log(error); });
    };



	$scope.newSubItemSirketBolumu = function (scope) {//yeni kayıt
		var nodeData = scope.$modelValue;
		_sirketBolumu = {
			id: '',
			idRef: nodeData.tabloId,
			bolumAdi: nodeData.Text + '/Güncelleyiniz.',
			status: true,
			sirket_Id: $scope.sirket.id
		};
		sirketSvc.InsertSirketBolumu(_sirketBolumu).then(function (response) {
			$scope.getSirketBolumuAllData(true);
		}, function (err) {
			//if (err.field && err.msg) {
			//} else {
			//}
		});
	};

	$scope.newSubSirketBolumu = function () {//yeni kayıt
		_sirketBolumu = {
			id: '',
			idRef: 0,
			bolumAdi: 'Yeni Bölüm-Kısım/Güncelleyiniz.',
			status: true,
			sirket_Id: $scope.sirket.id
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
	/////////////////////Atamalar\\\\\\\\\\\\\\\\\\\\\\
	$scope.person = {};
	$scope.yetkiler = [];//map çalışması için
	var servis = ngAuthSettings.apiServiceBaseUri;

	authService.fullUsers().then(function (roller) {
		$scope.rols = roller;
	});

	var uzmanPersonel = {
		uz_id: '',
		Id: '',
		Resim: '',
		Meslek: '',
		FullName: '',
		Email: '',
		Gorevi: '',
		Tel: '',
		JoinDate: ''
	};
	var resimx = '';
	$scope.getSirketAtamaAllData = function () {
		$scope.yetkiler = [];
		sirketSvc.GetSirketAtama($scope.sirket.id).then(function (data) {
			angular.forEach(data, function (uzman) {
				var index = $scope.rols.map(function (item) {
					return item.Id;
				}).indexOf(uzman.uzmanPersonelId);

				uploadService.GetImageId(uzman.uzmanPersonelId, "kimlik").then(function (data) {
					resimx =  data[0].LocalFilePath;
					uzmanPersonel = {
						uz_id: uzman.SirketAtama_id,
						Id: $scope.rols[index].Id,
						Resim: resimx,
						Meslek: $scope.rols[index].Meslek,
						FullName: $scope.rols[index].FullName,
						Email: $scope.rols[index].Email,
						Gorevi: $scope.rols[index].Gorevi,
						Tel: $scope.rols[index].Tel,
						JoinDate: $scope.rols[index].JoinDate
					};
					$scope.yetkiler.push(uzmanPersonel);
					$scope.count = $scope.yetkiler.length;
				});
			});

		});
	};

	$scope.add = function (prmtr) {
		var index = $scope.yetkiler.map(function (item) {
			return item.Id;//json taramasında kullanılır
		}).indexOf(prmtr.Id);
		if (index !== -1) {
			notify({
				message: 'Aynı Uzman/Hekim Sistemde Var!',
				classes: 'alert-danger',
				templateUrl: $scope.inspiniaTemplate,
				duration: 3000,
				position: 'right'
			});
		} else {
			sirketSvc.InsertSirketAtama({ SirketAtama_id: '', uzmanPersonelId: prmtr.Id, Sirket_id: $scope.sirket.id }).
				then(function (response) {
					uploadService.GetImageId(prmtr.Id, "kimlik").then(function (data) {
						//prmtr.Resim = servis + data[0].LocalFilePath + data[0].GenericName;
						  prmtr.Resim = data[0].LocalFilePath;
					});
					$scope.yetkiler.push(prmtr);
					$scope.count = $scope.yetkiler.length;
				}, function () {
					notify({
						message: 'Kayıt yapılamadı!',
						classes: 'alert-danger',
						templateUrl: $scope.inspiniaTemplate,
						duration: 3000,
						position: 'center'
					});
				});

		}
	};

	$scope.delete = function (prmtr) {
		sirketSvc.DeleteSirketAtama(prmtr.uz_id).then(function (response) {
			var index = $scope.yetkiler.indexOf(prmtr);
			if (index !== -1) {
				$scope.yetkiler.splice(index, 1);
				$scope.count = $scope.yetkiler.length;
			}
		}, function () {
			notify({
				message: 'Silinme işlemi yapılamadı!',
				classes: 'alert-danger',
				templateUrl: $scope.inspiniaTemplate,
				duration: 3000,
				position: 'center'
			});
		});
	};

	/////////////////////Detay Bilgileri\\\\\\\\\\\\\\\\\\\\
	var zx;
	$scope.tehlikeListesi = [{ 'n': 1, 'ac': 'Az Tehlikeli İşyeri Grubu' }, { 'n': 2, 'ac': 'Tehlikeli İşyeri Grubu' }, { 'n': 3, 'ac': 'Çok Tehlikeli İşyeri Grubu' }];
	$scope.getSirketDetayiAllData = function () {
		$scope.SirketDetayi = {
			'SirketDetayi_Id': '',
			'SGKSicilNo': '',
			'Telefon': '',
			'Faks': '',
			'Mail': '',
			'Adres': '',
			'ilMedullaKodu': '',
			'SirketYetkilisi': '',
			'SirketYetkilisiTcNo': '',
			'WebUrl': '',
			'WebUrlApi': '',
			'Nacekodu': '',
			'TehlikeGrubu': 0
		};

		sirketSvc.GetSirketDetayi($scope.sirket.id).then(function (data) {
			data === null ? $scope.SirketDetayi.SirketDetayi_Id = $scope.sirket.id : $scope.SirketDetayi = data;
			data === null ? zx = true : zx = false;
		});
	};
	$scope.savedSuccessfully = false;
	$scope.message = "";

	$scope.changeUp = function (prmtr) {
		if (zx) {
			sirketSvc.InsertSirketDetayi(prmtr).then(function (response) {
				$scope.savedSuccessfully = true;
				$scope.message = "Şirket Bilgileri başarıyla kayıt altına alınmıştır.";
				startTimer();
			},
				function (response) {
					var errors = [];
					for (var key in response.data.modelState) {
						for (var i = 0; i < response.data.modelState[key].length; i++) {
							errors.push(response.data.modelState[key][i]);
						}
					}
					$scope.message = errors.join(' ') + ":nedeniyle şirket kaydı başarısız olmuştur.";
				});
		}
		else {
			sirketSvc.UpdateSirketDetayi(prmtr.SirketDetayi_Id, prmtr).then(function (response) {
				$scope.savedSuccessfully = true;
				$scope.message = "Şirket Bilgileri başarıyla güncellenmiştir.";
				startTimer();
			},
				function (response) {
					var errors = [];
					for (var key in response.data.modelState) {
						for (var i = 0; i < response.data.modelState[key].length; i++) {
							errors.push(response.data.modelState[key][i]);
						}
					}
					$scope.message = errors.join(' ') + ":nedeniyle şirket kaydı başarısız olmuştur.";
				});
		}
	};

	/////////////////////Sağlık Birimi \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

	$scope.message = "";

	$scope.getSaglikBirimi = function () {
		sirketSvc.GetSaglikBirimi($scope.sirket.id).then(function (data) {
			$scope.sBirimleri = data;
			$scope.message = "";
            $scope.s_Birimi = { SaglikBirimi_Id: "", Adi: "", StiId: "", Protokol: "", Status: "", Year: "", EnableSsl: false, UseDefaultCredentials: false };
		}).catch(function (e) {
			$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
		});
		$scope.message = "Bekleyin Dosya Yüklemesi başladı....";
    };

    $scope.s_Birimi = { SaglikBirimi_Id: "", Adi: "", StiId: "", Protokol: "", Status: "", Year: "", EnableSsl: false, UseDefaultCredentials: false };

	$scope.newSaglikBirimi = function () {
        $scope.s_Birimi = { SaglikBirimi_Id: "", Adi: "", StiId: "", Protokol: "", Status: "", Year: "", EnableSsl: false, UseDefaultCredentials:false };
	};

	$scope.saveSaglikBirimi = function () {
		$scope.s_Birimi.Status = true;
		$scope.s_Birimi.StiId = $scope.sirket.id;
		if ($scope.s_Birimi.SaglikBirimi_Id === undefined || $scope.s_Birimi.SaglikBirimi_Id === "") {
			sirketSvc.InsertSaglikBirimi($scope.s_Birimi).then(function (response) {
				$scope.getSaglikBirimi();
			}).catch(function (e) {

			});
		} else {
			sirketSvc.UpdateSaglikBirimi($scope.s_Birimi.SaglikBirimi_Id, $scope.s_Birimi).then(function (response) {
				$scope.getSaglikBirimi();
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		}
	};

	$scope.deleteSaglikBirimi = function () {
		if ($scope.s_Birimi.SaglikBirimi_Id !== undefined || $scope.s_Birimi.SaglikBirimi_Id !== "") {
			$scope.s_Birimi.Status = false;
			$scope.s_Birimi.StiId = $scope.sirket.id;
			sirketSvc.UpdateSaglikBirimi($scope.s_Birimi.SaglikBirimi_Id, $scope.s_Birimi).then(function (response) {
				$scope.getSaglikBirimi();
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		}
	};
	//$scope.s_Birimi = { SaglikBirimi_Id: "", Adi: "", StiId: "", Protokol: "", Status: "" };
	$scope.satirBilgisi = function (aData) {
		$scope.s_Birimi.SaglikBirimi_Id = aData.SaglikBirimi_Id;
		$scope.s_Birimi.Adi = aData.Adi;
		$scope.s_Birimi.StiId = $scope.sirket.id;
		$scope.s_Birimi.Protokol = aData.Protokol;
		$scope.s_Birimi.Status = aData.Status;
		$scope.s_Birimi.Year = aData.Year;
		$scope.s_BirimiAdi = aData.Adi;
		$scope.s_Birimi.MailPort = aData.MailPort;
		$scope.s_Birimi.MailHost = aData.MailHost;
		$scope.s_Birimi.MailUserName = aData.MailUserName;
		$scope.s_Birimi.MailPassword = aData.MailPassword;
		$scope.s_Birimi.EnableSsl = aData.EnableSsl;
		$scope.s_Birimi.UseDefaultCredentials = aData.UseDefaultCredentials;
		$scope.s_Birimi.mailSekli = aData.mailSekli.trim();
		$scope.s_Birimi.mailfromAddress = aData.mailfromAddress.trim();
		$scope.s_Birimi.domain = aData.domain.trim();
	};
	var startTimer = function () {
		var timer = $timeout(function () {
			$timeout.cancel(timer);
			$scope.savedSuccessfully = false;
			$scope.message = "";
		}, 5000);
	};

	/////////////Mail Önerileri\\\\\\\\
	//vm.mailB = null;
	$scope.getMailListesi = function () {
		uploadService.GetImageStiId("logo", $scope.sirket.id).then(function(response) {
			$scope.picFile = response[0].LocalFilePath;
			$scope.resim = response[0];
		});
        $scope.Mail_Onerileri = null;
        sirketSvc.GetMailListesi($scope.sirket.id,$scope.bolum.tabloId).then(function (data) {
			$scope.Mail_Onerileri = data;
            $scope.message = "";
            $scope.totalItems = $scope.Mail_Onerileri.length;
		}).catch(function (e) {
			$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
		});
		$scope.message = "Bekleyin Dosya Yüklemesi başladı....";
	};

    $scope.satirKligi = function (index, mail_Onerileri) {
        $scope.selectedRow2 = index;
        vm.mailB = mail_Onerileri;
        vm.mailB.OneriTanimi = mail_Onerileri.OneriTanimi.trim();
        vm.mailB.MailAdresi = mail_Onerileri.MailAdresi.trim();
        vm.mailB.gonderimSekli = mail_Onerileri.gonderimSekli.trim();
    };


    $scope.viewby = 4;
    $scope.currentPage = 1;
    $scope.itemsPerPage = $scope.viewby;
    $scope.pageStartUp = function () {
        $scope.currentPage = 1;
    };
    $scope.setPage = function (pageNo) {
        $scope.currentPage = pageNo;
    };
    $scope.setItemsPerPage = function (num) {
        $scope.itemsPerPage = num;
    };
    $scope.selectedRow2 = -1;


    $scope.mailBilsisi = function (scope) {
        $scope.bolum = scope.$modelValue;
    };

	vm.onSubmit = function () {
        vm.mailB.Sirket_Id = $scope.sirket.id;
        vm.mailB.Bolum_Id = $scope.bolum.tabloId;
		if (vm.mailB.Mail_Onerileri_Id === undefined || vm.mailB.Mail_Onerileri_Id === "") {
			sirketSvc.InsertMail_Onerileri(vm.mailB).then(function (response) {
                $scope.Mail_Onerileri = null;
                $scope.getMailListesi();
				vm.mailB = {};
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		} else {
			sirketSvc.UpdateMail_Onerileri(vm.mailB.Mail_Onerileri_Id, vm.mailB).then(function (response) {
				$scope.getMailListesi();
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		}
	};

	vm.yeni = function () {
        vm.mailB = {};
        $scope.selectedRow2 = -1;
	};

    vm.sil = function () {
        $scope.selectedRow2 = -1;
		if (vm.mailB.Mail_Onerileri_Id !== undefined || vm.mailB.Mail_Onerileri_Id !== "") {
			sirketSvc.DeleteMail_Onerileri(vm.mailB.Mail_Onerileri_Id).then(function (response) {
				$scope.getMailListesi();
                vm.mailB = {};
                $scope.totalItems = $scope.Mail_Onerileri.length;
			}).catch(function (e) {
				$scope.message = "Hata Kontrol Edin! Tablo Yüklenemedi " + e;
			});
		}
	};
///////////////////İkon Ataması burada yapılacak
	$scope.progress = 0;
	var serviceBase = ngAuthSettings.apiServiceBaseUri;
	var serviceUploadApi = ngAuthSettings.apiUploadService;

	function replaceAll(str, from, to) {
		var idx = str.indexOf(from);

		while (idx > -1) {
			str = str.replace(from, to);
			idx = str.indexOf(from);
		}
		return str;
	}

	$scope.deleten = function () {
		if (!angular.isUndefined($scope.resim)) 
		{
			var genericFileName = replaceAll($scope.resim.GenericName, '.', '/');
			uploadService.DeleteStiFile(genericFileName).then(function (response) {
				if (response.data === 1) {
					$scope.resim = undefined;
				};
			});
		};
	};

	$scope.tamamlandi = false; $scope.tamamlandin = false;	
	$scope.upload = function (dataUrl) {
		$scope.tamamlandin = false;
		$scope.progress = 0;
		$scope.deleten();
		Upload.upload({
			url: serviceBase + serviceUploadApi + "PostSti" ,
			method: 'POST',
			data: {
				file: dataUrl,
				'DosyaBilgisi': {
					"DosyaTipi": "logo","DosyaTipiID" : "", "Konu": "Şirket logosu",
					"Hazırlayan": "Anonim","Sirket_Id" : $scope.sirket.id
				}
			}
		}).then(function (response) {
			$scope.tamamlandi = true; $scope.tamamlandin = true
			$scope.picFile = response.data[0].LocalFilePath;
			$scope.resim = response.data[0];
			$scope.progress = 0;
		}, function (response) {
			if (response.status > 0) $scope.errorMsg = response.status
				+ ': ' + response.data;
		}, function (evt) {
			$scope.progress = parseInt(100.0 * evt.loaded / evt.total);
			$scope.tamamlandin = false;
		});
	};
}

sirketlerCrtl.$inject = ['$scope', 'DTOptionsBuilder', 'Upload', 'sirketSvc',
	'authService', 'notify', 'uploadService', 'ngAuthSettings', '$timeout', '$filter'];

angular
	.module('inspinia')
	.controller('sirketlerCrtl', sirketlerCrtl);