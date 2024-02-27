'use strict';

function egtNotlariCtrl($scope, ngAuthSettings, personellerSvc, tanimlarSvc,
    DTOptionsBuilder, $stateParams, Upload, uploadService, $window) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var serviceUploadApi = ngAuthSettings.apiUploadService;
	var EgNC = this;
    var tableResize = window.innerWidth < 1200 ? true : false;
    $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withLanguageSource('/views/Personel/Controller/Turkish.json')
        .withDOM('<"html5buttons"B>lTfgitp')
        .withButtons([
            { extend: 'copy', text: 'Kopyala' },
            { extend: 'csv' },
            { extend: 'excel', title: 'DersListesi', name: 'Excel' },
            { extend: 'pdf', title: 'DersListesi', name: 'PDF' },
            {
                extend: 'print', text: 'Yazdır',
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
        .withOption('responsive', tableResize);

    if (!angular.isUndefined($stateParams.id)) {
        $scope.messageIcoEkle = true;
        uploadService.GetImageStiId("notlar", $stateParams.id ).then(function (data) {
            EgNC.materyaller = data;
        }).catch(function (e) {
            $scope.message = e.data;
        }).finally(function () {
            $scope.messageIcoEkle = false;
        });
    };
    $scope.download = function (f) {
        debugger;
        if (f.GenericName.indexOf('.') < 1) {
            var s = serviceBase + serviceUploadApi + 'stiDown/' + f.GenericName;
            $window.open(s, '_self', '');
        } else {
            window.open(f.LocalFilePath, 'popUpWindow',
                'height=1000,width=1200,left=10,top=10,,scrollbars=yes,menubar=no')
        }
    };

    EgNC.isgUsers = [];
    personellerSvc.GetFullIsgUsers().then(function (r) {//uzman listesi
        angular.forEach(r, function (value) {
            EgNC.isgUsers.push({ userId: value.Id, adi: value.FullName, gorevi: value.Gorevi !== undefined ? value.Gorevi : '', meslegi: value.Meslek !== undefined ? value.Meslek : '', TcNo: value.TcNo !== undefined ? value.TcNo : '' });
        });
    });
    EgNC.EgitimTurleri = [];
    tanimlarSvc.GetTanimAdiList("Personel Eğitim Tablosu").then(function (response) {
        angular.forEach(response, function (value, key) {
            EgNC.EgitimTurleri.push({
                konu: value.ifade !== null ? value.ifade.trim() : '',
                kod: value.tanimKisaltmasi !== null ? value.tanimKisaltmasi.trim() : '',
                tipi: value.ifadeBagimliligi !== null ? value.ifadeBagimliligi.trim() : '',
                tanimlama: value.ifadeBagimliligi.trim() + key
            });
        });
    });

    $scope.uploadi = function (files) {
        if (files && files.length == 1) {
            $scope.file = files[0];
            var URL = window.URL || window.webkitURL;
            var srcTmp = URL.createObjectURL($scope.file);
            $scope.materyal = files[0].name;
        }
    };
    $scope.tamamlandi = false; 
    $scope.upload = function () {
        $scope.tamamlandi = true; 
        if ((!angular.isUndefined($stateParams.id)) && EgNC.belgeTipi != null && EgNC.konu != null
            && EgNC.profesyoneli != null && $stateParams.id !== "0" && $scope.file != null) {
        Upload.upload({
            url: serviceBase + serviceUploadApi + "PostSti",
            method: 'POST',
            data: {
                file: $scope.file,
                'DosyaBilgisi': {
                    "DosyaTipi": "notlar", "DosyaTipiID": EgNC.belgeTipi, "Konu": EgNC.konu,
                    "Hazırlayan": EgNC.profesyoneli.adi,"Sirket_Id": $stateParams.id
                }
            }
        }).then(function (response) {
            $scope.tamamlandi = false; 
            $scope.materyal = response.data[0].FileName;
            $scope.file = response.data[0];
            uploadService.GetImageStiId("notlar", $stateParams.id).then(function (data) {
                EgNC.materyaller = data;
                EgNC.belgeTipi = null;
                EgNC.konu = null;
                EgNC.profesyoneli = null;
                $scope.file = null;
                $scope.materyal = null;
            });
        }, function (response) {
            if (response.status > 0) $scope.errorMsg = response.status
                + ': ' + response.data;
        });
        } else { alert("Şirket girişi veya bilgiler eksik."); $scope.tamamlandi = false; }
    };

    function replaceAll(str, from, to) {
        var idx = str.indexOf(from);

        while (idx > -1) {
            str = str.replace(from, to);
            idx = str.indexOf(from);
        }
        return str;
    }

    EgNC.Sil = function (s) {
        if (s.indexOf('.') > 0) { var genericFileName = replaceAll(s, '.', '/') }
        else { var genericFileName = s + '/x' };
            uploadService.DeleteStiFile(genericFileName).then(function (r) {
                if (r.data === 1) {
                    uploadService.GetImageStiId("notlar", $stateParams.id).then(function (data) {
                        EgNC.materyaller = data;
                    });
                };
            });
    };

}
egtNotlariCtrl.$inject = ['$scope', 'ngAuthSettings', 'personellerSvc', 'tanimlarSvc', 'DTOptionsBuilder',
    '$stateParams', 'Upload', 'uploadService','$window'];

angular
	.module('inspinia')
	.controller('egtNotlariCtrl', egtNotlariCtrl);