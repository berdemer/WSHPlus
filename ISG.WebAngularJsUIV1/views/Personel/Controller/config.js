'use strict';
function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {

    $urlRouterProvider.otherwise("/login");
    $ocLazyLoadProvider.config({
        debug: false
    });

    $stateProvider
        .state('index', {
            abstract: true,
            url: "/index",
            resolve: {
                //burada servisleri yazıyoruz.Controla gönderiyoruz.
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer(); //asenkron mimari için $q parametresini tanımlıyoruz.               
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json', //tipini bildiriyoruz.
                        method: 'GET', //metod burda post get delete vs
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        } //autorizasyon için token header kısmında bildiriliyor.
                    }).success(function (response) {
                        deferred.resolve(response); //asenkron çıkışın değerini burda belirtiyoruz.
                    }).error(function (error) {
                        return deferred.reject; //Asenkron çıkış hatalı ise kapatıyoruz
                    });
                    return deferred.promise; //Asenkron çıkış başarılı ise yayınlıyoruz.
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) { //resolvedeki fonksiyonu burada inject ediyoruz.
                $rootScope.FName = propertyData.FullName; //rootscope sayfa geçişlerinde kullanıyoruz.
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;
                z
            },
            templateUrl: "views/common/content.html"
        })
        //Autorizasyon bölümü
        .state('Account', {
            abstract: true,
            url: "/Account",
            templateUrl: "views/common/content.html",
            data: {
                permissions: {
                    only: ['Admin'],
                    redirectTo: 'login'
                }
            },
            resolve: {
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer();
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        }
                    }).then(function (response) {
                        deferred.resolve(response.data);
                    }, function (error) {
                        return deferred.reject;
                    });
                    return deferred.promise;
                },
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'cgNotify',
                            files: ['bower_components/angular-notify/dist/angular-notify.min.css',
                                'bower_components/angular-notify/dist/angular-notify.min.js']
                        }
                    ]);
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) {
                $rootScope.FName = propertyData.FullName;
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;

            }
        })
        .state('Account.UsersList', {
            url: "/usersList",
            templateUrl: "/views/Account/View/usersList.html",
            data: { pageTitle: 'Kullanıcılar Listesi' },
            controller: "usersListController",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Account/Controller/usersListController.js']
                        }
                    ]);
                }
            }
        })
        .state('Account.RegisterIn', {
            url: "/registerIn",
            templateUrl: "/views/Account/View/registerIn.html",
            data: { pageTitle: 'Kullancı Kaydı' },
            controller: "registerInController as vm",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['css/loading.css', 'views/Account/Controller/registerInController.js']
                        }
                    ]);
                }
            }
        })
        .state('Account.UserUpdate', {
            url: "/userUpdate/:id",
            templateUrl: "/views/Account/View/registerIn.html",
            data: { pageTitle: 'Kullanıcı Güncelleme' },
            controller: "registerInController as vm",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Account/Controller/registerInController.js']
                        }
                    ]);
                }
            }
        })
        .state('Account.UsersList.UserView', {
            url: "/userView/:id",
            templateUrl: "/views/Account/View/userView.html",
            data: { pageTitle: 'Kullanıcı Bilgisi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            serie: true,
                            files: ['bower_components/slick-carousel/slick/slick.css',
                                'bower_components/slick-carousel/slick/slick-theme.css',
                                'bower_components/slick-carousel/slick/slick.min.js']
                        },
                        {
                            name: 'slick',
                            files: ['bower_components/angular-slick/dist/angular-slick.min.js']
                        }
                    ]);
                }
            },
            controller: ['$scope', 'authService', '$stateParams', '$state', 'notify', 'uploadService', 'ngAuthSettings',
                function ($scope, authService, $stateParams, $state, notify, uploadService, ngAuthSettings) {
                    /*iç içe servis kullanıldı. asenkron teknolojisinde sıralı servis kullanıldığında
                    geçişlerde problem yaşanabilir. Bir servis tamamlanıp sonra kullanılması için aşağıdaki
                    örneği kullanmak gerekir.*/
                    $scope.servis = ngAuthSettings.apiServiceBaseUri;
                    function _getAllData() {
                        authService.userView($stateParams.id).then(function (data) {//birinci tamamladıkta sonra
                            $scope.idl = data;
                            $scope.Roller = data.Roles;
                            authService.GetAllRoles().then(function (roller) {//ikinci servis kullanılır.
                                $scope.rols = roller;
                                angular.forEach(data.Roles, function (UserRol) {//ikinci diziden çekilir.
                                    var index = $scope.rols.map(function (item) {
                                        return item.Name;//json taramasında kullanılır
                                    }).indexOf(UserRol);//rols dizinde hangi rol indexini bulur 
                                    if (index !== -1) {
                                        $scope.rols.splice(index, 1);
                                    }
                                });
                            });
                        });
                    }
                    _getAllData();

                    $scope.listImage = {};

                    uploadService.GetImageId($stateParams.id, "kimlik").then(function (data) {
                        $scope.listImage = data;
                    });

                    $scope.passwordUpdate = function (userId, newPassword) {
                        authService.adminUpdatePassword(userId, newPassword).then(function (resp) {
                            notify({
                                message: 'Başarılı bir şekilde güncellenmiştir.',
                                classes: 'alert-success',
                                templateUrl: $scope.inspiniaTemplate,
                                duration: 3000,
                                position: 'center'
                            });
                        });
                    };

                    $scope.rolex = {};
                    $scope.delete = function (role) {
                        var index = $scope.Roller.indexOf(role);

                        if (index !== -1) {
                            $scope.Roller.splice(index, 1);
                        }
                        $scope.rols.push({ Id: 'jhkjhkjh', Name: role });
                    };
                    $scope.add = function () {
                        var index = $scope.rols.map(function (item) {
                            return item.Name;//json taramasında kullanılır
                        }).indexOf($scope.rolex.Name);

                        if (index !== -1) {
                            $scope.rols.splice(index, 1);
                        }
                        $scope.Roller.push($scope.rolex.Name);
                        $scope.rolex = {};
                    };
                    $scope.inspiniaTemplate = 'views/common/notify.html';
                    $scope.assignRolesToUser = function () {
                        if ($scope.Roller.length > 0) {
                            authService.AssignRolesToUser($scope.idl.Id, $scope.Roller).then(function (response) {
                                notify({
                                    message: 'Başarılı bir şekilde güncellenmiştir.',
                                    classes: 'alert-success',
                                    templateUrl: $scope.inspiniaTemplate,
                                    duration: 3000,
                                    position: 'center'
                                });
                                $state.reload();//sayfayı tüm verileriyle günceller.
                            },
                            function (response) {
                                var errors = [];
                                for (var key in response.data.modelState) {
                                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                                        errors.push(response.data.modelState[key][i]);
                                    }
                                }
                                notify({
                                    message: 'Güncellenme yapılamadı..Hata! sonucu var',
                                    classes: 'alert-warning',
                                    templateUrl: $scope.inspiniaTemplate,
                                    duration: 8000,
                                    position: 'center'
                                });

                            });
                        } else {
                            notify({
                                message: 'Boş kayıt giriyorsunuz. Ekle sonra kayıt butonuna basınız.',
                                classes: 'alert-danger',
                                templateUrl: $scope.inspiniaTemplate,
                                duration: 2000,
                                position: 'center'
                            });
                        }
                    };

                }
            ]
        })
        .state('Account.RolesList', {
            url: "/rolesList",
            templateUrl: "/views/Account/View/rolesList.html",
            data: { pageTitle: 'Roller Listesi' },
            controller: "rolesListController as vm",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['css/loading.css', 'views/Account/Controller/rolesListController.js']
                        }
                    ]);
                }
            }
        })
        .state('login', {
            url: "/login",
            templateUrl: "/views/Account/View/login.html",
            data: { pageTitle: 'Kullanıcı Girişi' },
            controller: "loginController",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Account/Controller/loginController.js']
                        }
                    ]);
                }
            }
        })
        .state('register', {
            url: "/register",
            templateUrl: "/views/Account/View/register.html",
            data: { pageTitle: 'Kullanıcı Yeni Kayıt' },
            controller: "registerController",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Account/Controller/registerController.js']
                        }
                    ]);
                }
            }
        })
        //Dosya Gönder Bölümü
        .state('Upload', {
            abstract: true,
            url: "/Upload",
            templateUrl: "views/common/content.html",
            data: {
                permissions: {
                    only: ['Admin', 'ISG_Admin'],
                    redirectTo: 'login'
                }
            },
            resolve: {
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer();
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        }
                    }).then(function (response) {
                        deferred.resolve(response.data);
                    }, function (error) {
                        return deferred.reject;
                    });
                    return deferred.promise;
                },
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ngImgCrop',
                            files: ['bower_components/ngImgCrop/compile/minified/ng-img-crop.css',
                                'bower_components/ngImgCrop/compile/minified/ng-img-crop.js']
                        }
                    ]);
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) {
                $rootScope.FName = propertyData.FullName;
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;

            }
        })
        .state('Upload.FileManager', {
            url: "/FileManager/:id/:folder",
            templateUrl: "/views/Upload/View/FileManager.html",
            data: { pageTitle: 'Dosya Yükleme' },
            controller: "fileManagerController",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Upload/Controller/fileManagerController.js']
                        }
                    ]);
                }
            }
        })
        //Şirketlere Atama ve tanımlama Bölümü
        .state('Atama', {
            abstract: true,
            url: "/Atama",
            templateUrl: "views/common/content.html",
            data: {
                permissions: {
                    only: ['Admin', 'ISG_Admin', 'ISG_Hekim', 'ISG_SaglikMemuru'],
                    redirectTo: 'login'
                }
            },
            resolve: {
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer(); //             
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        }
                    }).then(function (response) {
                        deferred.resolve(response.data);
                    }, function (error) {
                        return deferred.reject;
                    });
                    return deferred.promise;
                },
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.tree',
                            files: ['bower_components/angular-ui-tree/dist/angular-ui-tree.min.css',
                                'bower_components/angular-ui-tree/dist/angular-ui-tree.min.js']
                        },
                        {
                            name: 'ui.select',
                            files: ['bower_components/angular-ui-select/dist/select.min.css',
                                'bower_components/angular-ui-select/dist/select.min.js']
                        },
                        {
                            name: 'cgNotify',
                            files: ['bower_components/angular-notify/dist/angular-notify.min.css',
                                'bower_components/angular-notify/dist/angular-notify.min.js']
                        },
                        {
                            files: ['bower_components/jasny-bootstrap/dist/js/jasny-bootstrap.min.js']
                        }
                    ]);
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) {
                $rootScope.FName = propertyData.FullName;
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;

            }
        })
        .state('Atama.sirketler', {
            url: "/sirketler",
            templateUrl: '/views/Atama/View/sirketler.html',
            controller: 'sirketlerCrtl',
            data: { pageTitle: 'Şirketler' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/sirketlerCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.sirketler.sirketBolumleri', {
            url: "/sirketBolumleri/:id/:name",
            templateUrl: "/views/Atama/View/sirketBolumleri.html",
            data: { pageTitle: 'Şirket Bölümleri' },
            controller: "sirketBolumleriCtrl as sc",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/sirketBolumleriCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.sirketAtamalari', {
            url: "/sirketAtamalari",
            templateUrl: "/views/Atama/View/sirketAtamalari.html",
            data: { pageTitle: 'Şirket Personel Atama' },
            controller: "sirketAtamalariCtrl as sc",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/sirketAtamalariCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.tanimlar', {
            url: "/tanimlar",
            templateUrl: "/views/Atama/View/tanimlar.html",
            data: {
                pageTitle: 'Tanimlamalar'
            },
            controller: "tanimlarCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/tanimlarCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.ilaclar', {
            url: "/ilaclar",
            templateUrl: "/views/Atama/View/ilaclar.html",
            data: { pageTitle: 'İlaçlar' },
            controller: "ilaclarCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/ilaclarCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.icd', {
            url: "/icd",
            templateUrl: "/views/Atama/View/icd.html",
            data: { pageTitle: 'ICD10' },
            controller: "icdCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/icdCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.rad', {
            url: "/rad",
            templateUrl: "/views/Atama/View/rad.html",
            data: { pageTitle: 'Radyoloji' },
            controller: "radCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/radCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.odio', {
            url: "/odio",
            templateUrl: "/views/Atama/View/odio.html",
            data: { pageTitle: 'Odio' },
            controller: "odioCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/odioCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.sft', {
            url: "/sft",
            templateUrl: "/views/Atama/View/sft.html",
            data: { pageTitle: 'SFT' },
            controller: "sftCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/sftCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.hemogram', {
            url: "/hemogram",
            templateUrl: "/views/Atama/View/hemogram.html",
            data: { pageTitle: 'Hemogram' },
            controller: "hemogramCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/hemogramCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.biyokimya', {
            url: "/biyokimya",
            templateUrl: "/views/Atama/View/biyokimya.html",
            data: { pageTitle: 'Biyokimya' },
            controller: "biyokimyaCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/biyokimyaCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Atama.resimler', {
            url: "/resimler",
            templateUrl: "/views/Atama/View/resimler.html",
            data: { pageTitle: 'ICD10' },
            controller: "resimlerCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Atama/Controller/resimlerCtrl.js']
                        }
                    ]);
                }
            }
        })
        //personel işlemleri bölümü
        .state('Personel', {
            abstract: true,
            url: "/Personel",
            templateUrl: "views/common/content.html",
            data: {
                permissions: {
                    only: ['Admin', 'ISG_Admin', 'ISG_Hekim', 'ISG_SaglikMemuru'],
                    redirectTo: 'login'
                }
            },
            resolve: {
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer();
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        }
                    }).then(function (response) {
                        deferred.resolve(response.data);
                    }, function (error) {
                        return deferred.reject;
                    });
                    return deferred.promise;
                },
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'cgNotify',
                            files: ['bower_components/angular-notify/dist/angular-notify.min.css',
                                'bower_components/angular-notify/dist/angular-notify.min.js']
                        },
                        {
                            name: 'ui.select',
                            files: ['bower_components/angular-ui-select/dist/select.min.css',
                                'bower_components/angular-ui-select/dist/select.min.js']
                        },
                        {
                            name: 'treeControl',
                            files: ['bower_components/angular-tree-control/css/tree-control-attribute.css',
                                    'bower_components/angular-tree-control/css/tree-control.css',
                                    "bower_components/angular-tree-control/angular-tree-control.js"]
                        },
                        {
                            insertBefore: '#loadBefore',
                            files: ['bower_components/jquery-steps/demo/css/jquery.steps.css']
                        }
                    ]);
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) {
                $rootScope.FName = propertyData.FullName;
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;
            },
            onExit: function (personellerSvc) {
                personellerSvc.MoviesIds.BolumId = 0;
                personellerSvc.MoviesIds.SirketId = 0;
                personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
                personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
            }
        })
        .state('Personel.PersonellerList', {
            url: "/personellerList",
            templateUrl: "/views/Personel/View/personellerList.html",
            data: { pageTitle: 'Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/personellerListCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.PersonellerListDetail', {
            url: "/personellerListDetail/:sti/:blm/:stiAd/:blmAd",
            templateUrl: "/views/Personel/View/personellerList.html",
            data: { pageTitle: 'Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/personellerListCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.PersonellerFullListDetail', {
            url: "/personellerFullListDetail/:sd",
            templateUrl: "/views/Personel/View/personellerList.html",
            data: { pageTitle: 'Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/personellerListCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.excelYukle', {
            url: "/excelYukle",
            templateUrl: "/views/Personel/View/excelYukle.html",
            data: {
                pageTitle: 'Excelden Veri Alımı'
            },
            controller: "excelYukleCtrl",
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/excelYukleCtrl.js']
                        }
                    ]);
                }
            },
            onEnter: function (personellerSvc) {
                personellerSvc.MoviesIds.BolumId = 0;
                personellerSvc.MoviesIds.SirketId = 0;
                personellerSvc.MoviesIds.SirketAdi = 'Şirketi Seçiniz.!';
                personellerSvc.MoviesIds.BolumAdi = 'Bölümü Seçiniz.!';
            }
        })
        .state('Personel.PersonelNew', {
            url: "/new",
            templateUrl: "/views/Personel/View/PersDetail/personelNew.html",
            data: { pageTitle: 'Personel Yeni Kayıt' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/personelViewCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers', {
            url: "/pers",
            templateUrl: "/views/Personel/View/pers.html",
            data: { pageTitle: 'Personel Guncelleme' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/personelViewCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.GenericUpdate', {
            url: "/genericUpdate/:id",
            templateUrl: "/views/Personel/View/PersDetail/genericUpdate.html",
            data: { pageTitle: 'Personel Güncellenmesi' }
        })
        .state('Personel.Pers.AdressUpdate', {
            url: "/adressUpdate/:id",
            templateUrl: "/views/Personel/View/PersDetail/adressUpdate.html",
            data: { pageTitle: 'Adres' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/adressUpdateCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.CalismaDurumuUpdate', {
            url: "/calismaDurumuUpdate/:id",
            templateUrl: "/views/Personel/View/PersDetail/calismaDurumuUpdate.html",
            data: { pageTitle: 'Çalışma Durumu' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/calismaDurumuUpdateCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.CalismaGecmisi', {
            url: "/calismaGecmisi/:id",
            templateUrl: "/views/Personel/View/PersDetail/calismaGecmisi.html",
            data: { pageTitle: 'Çalışma Geçmişi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/calismaGecmisiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.EgitimHayati', {
            url: "/egitimHayati/:id",
            templateUrl: "/views/Personel/View/PersDetail/egitimHayati.html",
            data: { pageTitle: 'Eğitim Hayatı' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/egitimHayatiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.DisRapor', {
            url: "/disRapor/:id",
            templateUrl: "/views/Personel/View/PersDetail/disRapor.html",
            data: { pageTitle: 'Dış Raporları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/disRaporCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.IcRapor', {
            url: "/icRapor/:id",
            templateUrl: "/views/Personel/View/PersDetail/icRapor.html",
            data: { pageTitle: 'İç Raporları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/icRaporCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.Ozurluluk', {
            url: "/ozurluluk/:id",
            templateUrl: "/views/Personel/View/PersDetail/ozurluluk.html",
            data: { pageTitle: 'Özürlülükleri' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/ozurlulukCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.IsgEgitimi', {
            url: "/isgEgitimi/:id",
            templateUrl: "/views/Personel/View/PersDetail/isgEgitimi.html",
            data: { pageTitle: 'İsg Eğitimleri' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/isgEgitimiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Personel.Pers.Kkd', {
            url: "/kkd/:id",
            templateUrl: "/views/Personel/View/PersDetail/Kkd.html",
            data: { pageTitle: 'Kişisel Koruyucu Donanımlar' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/kkdCtrl.js']
                        }
                    ]);
                }
            }
        })
        //Sağlık Merkezi Bölümü
        .state('SM', {
            abstract: true,
            url: "/sm",
            templateUrl: "views/common/content.html",
            data: {
                permissions: {
                    only: ['Admin', 'ISG_Admin', 'ISG_Hekim', 'ISG_SaglikMemuru'],
                    redirectTo: 'login'
                }
            },
            resolve: {
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer();
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        }
                    }).then(function (response) {
                        deferred.resolve(response.data);
                    }, function (error) {
                        return deferred.reject;
                    });
                    return deferred.promise;
                },
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'cgNotify',
                            files: ['bower_components/angular-notify/dist/angular-notify.min.css',
                                'bower_components/angular-notify/dist/angular-notify.min.js']
                        },
                        {
                            name: 'ui.select',
                            files: ['bower_components/angular-ui-select/dist/select.min.css',
                                'bower_components/angular-ui-select/dist/select.min.js']
                        },
                        {
                            name: 'treeControl',
                            files: ['bower_components/angular-tree-control/css/tree-control-attribute.css',
                                    'bower_components/angular-tree-control/css/tree-control.css',
                                    "bower_components/angular-tree-control/angular-tree-control.js"]
                        },
                        {
                            files: ['bower_components/Chart-js/dist/Chart.min.js']
                        },
                        {
                            name: 'angles',
                            files: ['bower_components/angles-master/angles.js']
                        },
                        {
                            insertBefore: '#loadBefore',
                            files: ['bower_components/jquery-steps/demo/css/jquery.steps.css']
                        },
                        {
                            files: ['bower_components/bootstrap-social/bootstrap-social.css']
                        }
                    ]);
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) {
                $rootScope.FName = propertyData.FullName;
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;
            }
        })
        .state('SM.SMList', {
            url: "/SMList",
            templateUrl: "views/SM/View/SMList.html",
            data: { pageTitle: 'Sağlık Merkezi Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/SMListCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.SMCards', {
            url: "/SMCards/:id",
            templateUrl: "views/SM/View/SMCards.html",
            data: { pageTitle: 'P. Kartları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/SMCardsCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.StokTanimla', {
            url: "/StokTanimla",
            templateUrl: "views/SM/View/StokTanimla.html",
            data: { pageTitle: 'Stok Tanımları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/StokTanimlaCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.StokHareketi', {
            url: "/StokHareketi",
            templateUrl: "views/SM/View/StokHareketi.html",
            data: { pageTitle: 'Stok Hareketleri' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/StokHareketiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og', {
            url: "/Og",
            templateUrl: "views/SM/View/Og.html",
            data: { pageTitle: 'ÖzGeçmiş' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/OgCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og.Allerji', {
            url: "/allerji/:id",
            templateUrl: "views/SM/View/Ozg/allerji.html",
            data: { pageTitle: 'Allerji Öyküsü' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Ozg/allerjiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og.Asi', {
            url: "/asi/:id",
            templateUrl: "views/SM/View/Ozg/asi.html",
            data: { pageTitle: 'Aşılamaları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Ozg/asiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og.KronikHastalik', {
            url: "/kronikHastalik/:id",
            templateUrl: "views/SM/View/Ozg/kronikHastalik.html",
            data: { pageTitle: 'Kronik Hastalıkları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Ozg/kronikHastalikCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og.Aliskanlik', {
            url: "/aliskanlik/:id",
            templateUrl: "views/SM/View/Ozg/aliskanlik.html",
            data: { pageTitle: 'Alışkanlıkları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Ozg/aliskanlikCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og.Ozurluluk', {
            url: "/ozurluluk/:id",
            templateUrl: "/views/Personel/View/PersDetail/ozurluluk.html",
            data: { pageTitle: 'Özürlülük Durumları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/ozurlulukCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Og.SoyGecmisi', {
            url: "/soyGecmisi/:id",
            templateUrl: "views/SM/View/Ozg/soyGecmisi.html",
            data: { pageTitle: 'Soy Geçmişi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Ozg/soyGecmisiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk', {
            url: "/Tk",
            templateUrl: "views/SM/View/Tk.html",
            data: { pageTitle: 'Tetkikler' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/TkCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Sft', {
            url: "/sft/:id",
            templateUrl: "views/SM/View/Tk/sft.html",
            data: { pageTitle: 'Solunm Fonksiyon Testi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/sftCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Odio', {
            url: "/odio/:id",
            templateUrl: "views/SM/View/Tk/odio.html",
            data: { pageTitle: 'Odio' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/odioCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Laboratuar', {
            url: "/laboratuar/:id",
            templateUrl: "views/SM/View/Tk/laboratuar.html",
            data: { pageTitle: 'Laboratuvar' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/laboratuarCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.TA', {
            url: "/ta/:id",
            templateUrl: "views/SM/View/Tk/ta.html",
            data: { pageTitle: 'Tansiyon Nabız' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/taCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.BoyKilo', {
            url: "/boyKilo/:id",
            templateUrl: "views/SM/View/Tk/boyKilo.html",
            data: { pageTitle: 'Boy Kilo BKM index' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/boyKiloCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Hemogram', {
            url: "/hemogram/:id",
            templateUrl: "views/SM/View/Tk/hemogram.html",
            data: { pageTitle: 'Hemogram' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/hemogramCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Radyoloji', {
            url: "/radyoloji/:id",
            templateUrl: "views/SM/View/Tk/radyoloji.html",
            data: { pageTitle: 'Radyoloji' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/radyolojiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Ekg', {
            url: "/ekg/:id",
            templateUrl: "views/SM/View/Tk/ekg.html",
            data: { pageTitle: 'Elektrokardiografi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/ekgCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Gorme', {
            url: "/gorme/:id",
            templateUrl: "views/SM/View/Tk/gorme.html",
            data: { pageTitle: 'Rutin Görme Muayenesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/gormeCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.Pansuman', {
            url: "/pansuman/:id",
            templateUrl: "views/SM/View/Tk/pansuman.html",
            data: { pageTitle: 'Pansumanları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/pansumanCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.RevirTedavi', {
            url: "/revirTedavi/:id",
            templateUrl: "views/SM/View/Tk/revirTedavi.html",
            data: { pageTitle: 'Revir Tedavileri' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/revirTedaviCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Tk.PsikolojikTest', {
            url: "/psikolojikTest/:id",
            templateUrl: "views/SM/View/Tk/psikolojikTest.html",
            data: { pageTitle: 'Psikolojik Testler' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/Tk/psikolojikTestCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Pm', {
            url: "/Pm/:id",
            templateUrl: "views/SM/View/Pm.html",
            data: { pageTitle: 'Muayene' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/PmCtrl.js']
                        },
                        {
                            files: ['bower_components/clockpicker/dist/bootstrap-clockpicker.min.css',
                                'bower_components/clockpicker/dist/bootstrap-clockpicker.min.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Prm', {
            url: "/Prm/:id",
            templateUrl: "views/SM/View/Prm.html",
            data: { pageTitle: 'Periyodik Muayene' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/PrmCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Ik', {
            url: "/Ik/:id",
            templateUrl: "views/SM/View/Ik.html",
            data: { pageTitle: 'İş Kazası' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/IkCtrl.js']
                        },
                        {
                            name: 'mgo-angular-wizard',
                            files: ['bower_components/angular-wizard/dist/angular-wizard.min.css',
                                'bower_components/angular-wizard/dist/angular-wizard.min.js']
                        }
                        ,
                        {
                            files: ['bower_components/clockpicker/dist/bootstrap-clockpicker.min.css',
                                'bower_components/clockpicker/dist/bootstrap-clockpicker.min.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb', {
            url: "/gb",
            templateUrl: "views/SM/View/Gb.html",
            data: { pageTitle: 'Personel Guncelleme' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/SM/Controller/gbCtrl.js', 'views/Personel/Controller/personelViewCtrl.js']
                        }
                    ]);
                }
            }
        })//Genel Bilgileri
        .state('SM.Gb.GenericUpdate', {
            url: "/genericUpdate/:id",
            templateUrl: "/views/Personel/View/PersDetail/genericUpdate.html",
            data: { pageTitle: 'Personel Güncellenmesi' }
        })
        .state('SM.Gb.AdressUpdate', {
            url: "/adressUpdate/:id",
            templateUrl: "/views/Personel/View/PersDetail/adressUpdate.html",
            data: { pageTitle: 'Adres' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/adressUpdateCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.CalismaDurumuUpdate', {
            url: "/calismaDurumuUpdate/:id",
            templateUrl: "/views/Personel/View/PersDetail/calismaDurumuUpdate.html",
            data: { pageTitle: 'Çalışma Durumu' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/calismaDurumuUpdateCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.CalismaGecmisi', {
            url: "/calismaGecmisi/:id",
            templateUrl: "/views/Personel/View/PersDetail/calismaGecmisi.html",
            data: { pageTitle: 'Çalışma Geçmişi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/calismaGecmisiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.EgitimHayati', {
            url: "/egitimHayati/:id",
            templateUrl: "/views/Personel/View/PersDetail/egitimHayati.html",
            data: { pageTitle: 'Eğitim Hayatı' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/egitimHayatiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.DisRapor', {
            url: "/disRapor/:id",
            templateUrl: "/views/Personel/View/PersDetail/disRapor.html",
            data: { pageTitle: 'Dış Raporları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/disRaporCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.IcRapor', {
            url: "/icRapor/:id",
            templateUrl: "/views/Personel/View/PersDetail/icRapor.html",
            data: { pageTitle: 'İç Raporları' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/icRaporCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.Ozurluluk', {
            url: "/ozurluluk/:id",
            templateUrl: "/views/Personel/View/PersDetail/ozurluluk.html",
            data: { pageTitle: 'Özürlülükleri' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/ozurlulukCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.IsgEgitimi', {
            url: "/isgEgitimi/:id",
            templateUrl: "/views/Personel/View/PersDetail/isgEgitimi.html",
            data: { pageTitle: 'İsg Eğitimleri' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/isgEgitimiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('SM.Gb.Kkd', {
            url: "/kkd/:id",
            templateUrl: "/views/Personel/View/PersDetail/Kkd.html",
            data: { pageTitle: 'Kişisel Koruyucu Donanımlar' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Personel/Controller/kkdCtrl.js']
                        }
                    ]);
                }
            }
        })
        //Analiz bölümü
        .state('Analiz', {
            abstract: true,
            url: "/analiz",
            templateUrl: "views/common/content.html",
            data: {
                permissions: {
                    only: ['Admin', 'ISG_Admin', 'ISG_Hekim', 'ISG_SaglikMemuru'],
                    redirectTo: 'login'
                }
            },
            resolve: {
                propertyData: function ($http, $q, authService) {
                    var deferred = $q.defer();
                    $http({
                        url: serviceBase + 'api/accounts/user/' + authService.authentication.id,
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "Accept": "application/json",
                            "Authorization": "Bearer " + authService.authentication.token
                        }
                    }).then(function (response) {
                        deferred.resolve(response.data);
                    }, function (error) {
                        return deferred.reject;
                    });
                    return deferred.promise;
                },
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'cgNotify',
                            files: ['bower_components/angular-notify/dist/angular-notify.min.css',
                                'bower_components/angular-notify/dist/angular-notify.min.js']
                        },
                        {
                            name: 'ui.select',
                            files: ['bower_components/angular-ui-select/dist/select.min.css',
                                'bower_components/angular-ui-select/dist/select.min.js']
                        },
                        {
                            name: 'treeControl',
                            files: ['bower_components/angular-tree-control/css/tree-control-attribute.css',
                                    'bower_components/angular-tree-control/css/tree-control.css',
                                    "bower_components/angular-tree-control/angular-tree-control.js"]
                        },
                        {
                            files: ['bower_components/Chart-js/dist/Chart.min.js']
                        },
                        {
                            name: 'angles',
                            files: ['bower_components/angles-master/angles.js']
                        },
                        {
                            insertBefore: '#loadBefore',
                            files: ['bower_components/jquery-steps/demo/css/jquery.steps.css']
                        },
                        {
                            files: ['bower_components/bootstrap-social/bootstrap-social.css']
                        }
                    ]);
                }
            },
            controller: function ($scope, propertyData, $rootScope, uploadService, ngAuthSettings) {
                $rootScope.FName = propertyData.FullName;
                $rootScope.Gorevi = propertyData.Gorevi;
                uploadService.GetImageId(propertyData.Id, 'kimlik').then(function (data) {
                    $rootScope.Img = data[0];
                });
                $rootScope.servisen = ngAuthSettings.apiServiceBaseUri;
            }
        })
        .state('Analiz.Ramazzinis', {
            url: "/ramazzini/:sti/:blm/:sAdi/:bAdi",
            templateUrl: "views/Analiz/View/Ramazzini.html",
            data: { pageTitle: 'Ramazzini' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/RamazziniCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.Ramazzini', {
            url: "/ramazzini",
            templateUrl: "views/Analiz/View/Ramazzini.html",
            data: { pageTitle: 'Ramazzini' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/RamazziniCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.YillikRapor', {
            url: "/yillikRapor",
            templateUrl: "views/Analiz/View/YillikRapor.html",
            data: { pageTitle: 'Yıllık Değerlendirme' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/yillikRaporCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.IsKazasiListesi', {
            url: "/isKazasiListesi",
            templateUrl: "views/Analiz/View/IsKazasiListesi.html",
            data: { pageTitle: 'İş Kazası Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/isKazasiListesiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.RevirIslemleri', {
            url: "/revirIslemleri",
            templateUrl: "views/Analiz/View/RevirIslemleri.html",
            data: { pageTitle: 'Revir İşlemleri Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/revirIslemleriCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.BolumAnalizi', {
            url: "/bolumAnalizi",
            templateUrl: "views/Analiz/View/BolumAnalizi.html",
            data: { pageTitle: 'İş Kazası Bölum Analizi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/bolumAnaliziCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.PeriyodikMuayene', {
            url: "/periyodikMuayene",
            templateUrl: "views/Analiz/View/PeriyodikMuayene.html",
            data: { pageTitle: 'Periyodik Muayene Analizi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/periyodikMuayeneCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.Asilar', {
            url: "/asilar",
            templateUrl: "views/Analiz/View/Asilar.html",
            data: { pageTitle: 'Aşı Tarihleri Analizi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/asilarCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.DisRapor', {
            url: "/disRapor",
            templateUrl: "views/Analiz/View/DisRapor.html",
            data: { pageTitle: 'Dış Raporların Analizi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/disRaporCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.EngelliIslemleri', {
            url: "/engelliIslemleri",
            templateUrl: "views/Analiz/View/EngelliIslemleri.html",
            data: { pageTitle: 'Engelli Olan Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/engelliIslemleriCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.KronikHastaTakibi', {
            url: "/kronikHastaTakibi",
            templateUrl: "views/Analiz/View/KronikHastaTakibi.html",
            data: { pageTitle: 'Kronik Hastalıkları Olan Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/kronikHastaTakibiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.AllerjiHastaTakibi', {
            url: "/allerjiHastaTakibi",
            templateUrl: "views/Analiz/View/AllerjiHastaTakibi.html",
            data: { pageTitle: 'Kronik Hastalıkları Olan Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/allerjiHastaTakibiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.AliskanlikHastaTakibi', {
            url: "/aliskanlikHastaTakibi",
            templateUrl: "views/Analiz/View/AliskanlikHastaTakibi.html",
            data: { pageTitle: 'Bağımlı Olan Personel Listesi' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/aliskanlikHastaTakibiCtrl.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.Profil', {
            url: "/profil",
            templateUrl: "views/Analiz/View/Profil.html",
            data: { pageTitle: 'Profiliniz' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/profilCtrl.js']
                        },
                        {
                            serie: true,
                            files: ['bower_components/slick-carousel/slick/slick.css',
                                'bower_components/slick-carousel/slick/slick-theme.css',
                                'bower_components/slick-carousel/slick/slick.min.js']
                        },
                        {
                            name: 'slick',
                            files: ['bower_components/angular-slick/dist/angular-slick.min.js']
                        }
                    ]);
                }
            }
        })
        .state('Analiz.Imza', {
            url: "/imzaKaydi",
            templateUrl: "views/Analiz/View/Imza.html",
            data: { pageTitle: 'E-İmza bilgileriniz' },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            insertBefore: '#loadBefore',
                            files: ['views/Analiz/Controller/imzaCtrl.js']
                        }
                    ]);
                }
            }
        })
    ;
}


function config2(formlyConfigProvider) {
    formlyConfigProvider.setType([
      {
          name: 'input',
          templateUrl: 'input-template.html'
      },
      {
          name: 'checkbox',
          templateUrl: 'checkbox-template.html'
      }
    ]);
    formlyConfigProvider.setWrapper([
        {
            template: [
                '<div class="formly-template-wrapper form-group"',
                'ng-class="{\'has-error\': options.validation.errorExistsAndShouldBeVisible}">',
                '<label for="{{::id}}">{{options.templateOptions.label}} {{options.templateOptions.required ? \'*\' : \'\'}}</label>',
                '<formly-transclude></formly-transclude>',
                '<div class="validation"',
                'ng-if="options.validation.errorExistsAndShouldBeVisible"',
                'ng-messages="options.formControl.$error">',
                '<div ng-messages-include="validation.html"></div>',
                '<div ng-message="{{::name}}" ng-repeat="(name, message) in ::options.validation.messages">',
                '{{message(options.formControl.$viewValue, options.formControl.$modelValue, this)}}',
                '</div>',
                '</div>',
                '</div>'
            ].join(' '),
            types: 'maskedInput'
        },
        {
            template: [
                '<div class="formly-template-wrapper form-group"',
                'ng-class="{\'has-error\': options.validation.errorExistsAndShouldBeVisible}">',
                '<label for="{{::id}}">{{options.templateOptions.label}} {{options.templateOptions.required ? \'*\' : \'\'}}</label>',
                '<formly-transclude></formly-transclude>',
                '<div class="validation"',
                'ng-if="options.validation.errorExistsAndShouldBeVisible"',
                'ng-messages="options.formControl.$error">',
                '<div ng-messages-include="validation.html"></div>',
                '<div ng-message="{{::name}}" ng-repeat="(name, message) in ::options.validation.messages">',
                '{{message(options.formControl.$viewValue, options.formControl.$modelValue, this)}}',
                '</div>',
                '</div>',
                '</div>'
            ].join(' '),
            types: 'input'
        },
        {
            template: [
                '<div class="checkbox formly-template-wrapper-for-checkboxes form-group">',
                '<label for="{{::id}}">',
                '<formly-transclude></formly-transclude>',
                '</label>',
                '</div>'
            ].join(' '),
            types: 'checkbox'
        }
    ]);
}

function config3(formlyConfigProvider, formlyExampleApiCheck) {
    // set templates here
    formlyConfigProvider.setType({
        name: 'matchField',
        apiCheck: function () {
            return {
                data: {
                    fieldToMatch: formlyExampleApiCheck.string
                }
            };
        },
        apiCheckOptions: {
            prefix: 'matchField type'
        },
        defaultOptions: function matchFieldDefaultOptions(options) {
            return {
                extras: {
                    validateOnModelChange: true
                },
                expressionProperties: {
                    'templateOptions.disabled': function (viewValue, modelValue, scope) {
                        var matchField = find(scope.fields, 'key', options.data.fieldToMatch);
                        if (!matchField) {
                            throw new Error('Anahtar icin bir alan bulunamadi ' + options.data.fieldToMatch);
                        }
                        var model = options.data.modelToMatch || scope.model;
                        var originalValue = model[options.data.fieldToMatch];
                        var invalidOriginal = matchField.formControl && matchField.formControl.$invalid;
                        return !originalValue || invalidOriginal;
                    }
                },
                validators: {
                    fieldMatch: {
                        expression: function (viewValue, modelValue, fieldScope) {
                            var value = modelValue || viewValue;
                            var model = options.data.modelToMatch || fieldScope.model;
                            return value === model[options.data.fieldToMatch];
                        },
                        message: options.data.matchFieldMessage || '"Eslesmek zorunda"'
                    }
                }
            };

            function find(array, prop, value) {
                var foundItem;
                array.some(function (item) {
                    if (item[prop] === value) {
                        foundItem = item;
                    }
                    return !!foundItem;
                });
                return foundItem;
            }
        }
    });
}

function config4(formlyConfigProvider) {

    formlyConfigProvider.setWrapper({
        name: 'loader',
        template: [
          '<formly-transclude></formly-transclude>',
          '<span class="glyphicon glyphicon-refresh loader" ng-show="to.loading"></span>'
        ].join(' ')
    });

    formlyConfigProvider.setType({
        name: 'input-loader',
        extends: 'input',
        wrapper: ['loader']
    });

    formlyConfigProvider.setWrapper({
        template: '<formly-transclude></formly-transclude><div my-messages="options"></div>',
        types: ['input', 'checkbox', 'select', 'textarea', 'radio', 'input-loader']
    });
}

var uploadFolder = 'uploads/';

function config5(formlyConfigProvider) {
    formlyConfigProvider.setType({
        name: 'custom',
        templateUrl: 'custom.html'
    });
}
//local bir projede çalışacaklar

var uploadApi = 'api/Img/';
var serviceBase = 'https://api.cottonby.com/';
var storageLink = 'https://cottonby.com/';
var IsAzureProject = false;
var eimzaLink = 'http://localhost:8085/';
//azurda çalışaçaklar
//var serviceBase = 'http://isgsplus.azurewebsites.net/';
//var uploadApi = 'api/azurDepo/';
//var storageLink = 'https://isgdepo.blob.core.windows.net/';
//var IsAzureProject = true;
//var uploadApi = 'api/Img/';
//var serviceBase = 'http://localhost/webApp/';
//var storageLink = 'http://localhost/webApp/';
//var IsAzureProject = false;
//var eimzaLink = 'http://localhost:8085/';

angular
    .module('inspinia')
    .config(config)
    .config(config5)
    .config(config2)
    .config(config3)
    .config(config4)
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
        // $httpProvider.defaults.useXDomain = true;
        //  delete $httpProvider.defaults.headers.common['X-Requested-With'];
    })
    .config([
        '$controllerProvider', function ($controllerProvider) {
            $controllerProvider.allowGlobals();
        }
    ])
    .config([
        '$resourceProvider', function ($resourceProvider) {
            // Don't strip trailing slashes from calculated URLs
            $resourceProvider.defaults.stripTrailingSlashes = false;
        }
    ])
    .constant('ngAuthSettings', {//Otomasyon link servisi
        apiServiceBaseUri: serviceBase,
        clientId: 'ngAuthApp',
        apiUploadService: uploadApi,
        storageLinkService: storageLink,
        isAzure: IsAzureProject,
        uploadFolder: uploadFolder,
        eimzaLinki: eimzaLink
    })
    .constant('appApiCheck', apiCheck({
        output: { prefix: 'formlyExampleApp' }
    }))
    .constant('formlyExampleApiCheck', apiCheck())
    .run(function ($rootScope, $state, authService, Permission, $q) {
        $rootScope.$state = $state;//standart roller verilecek...
        var systemRoles = ['Admin', 'ISG_Admin', 'ISG_Hekim', 'ISG_SaglikMemuru', 'ISG_Denetci',
            'ISG_Memur', 'ISG_Uzman', 'IK_Memur', 'IK_Mudur', 'Bol_Amiri', 'Bol_Memuru'];
        Permission.defineManyRoles(systemRoles, function (stateParams, role) {
            var deferred = $q.defer();
            authService.fillAuthData();
            var userRoles = authService.authentication.roles;
            if (userRoles === undefined || userRoles === null) userRoles = [];
            var userHasRole = userRoles.indexOf(role) !== -1;
            userHasRole ? deferred.resolve() : deferred.reject();
            return deferred.promise;
        });
    })
    .run(function (formlyConfig, appApiCheck) {
        formlyConfig.setType({
            name: 'maskedInput',
            extends: 'input',
            template: '<input class="form-control" ng-model="model[options.key]" />',
            defaultOptions: {
                ngModelAttrs: {
                    mask: {
                        attribute: 'ui-mask'
                    },
                    maskPlaceholder: {
                        attribute: 'ui-mask-placeholder'
                    }
                },
                templateOptions: {
                    maskPlaceholder: ''
                }
            }
        });
    })
    .run(function (formlyConfig) {
        formlyConfig.extras.removeChromeAutoComplete = true;
        formlyConfig.setType({
            name: 'ui-select',
            extends: 'select',
            template: '<ui-select ng-model="model[options.key]" theme="bootstrap" ng-required="{{to.required}}" ng-disabled="{{to.disabled}}" reset-search-input="false"> <ui-select-match placeholder="{{to.placeholder}}"> {{$select.selected[to.labelProp || \'name\']}} </ui-select-match> <ui-select-choices group-by="to.groupBy" repeat="option[to.valueProp || \'value\'] as option in to.options | filter: $select.search"> <div ng-bind-html="option[to.labelProp || \'name\'] | highlight: $select.search"></div> </ui-select-choices> </ui-select>'
        });
        // Configure custom types
        formlyConfig.setType({
            name: 'ui-select-single',
            extends: 'select',
            templateUrl: 'ui-select-single.html'
        });
        formlyConfig.setType({
            name: 'ui-select-single-select2',
            extends: 'select',
            templateUrl: 'ui-select-single-select2.html'
        });
        formlyConfig.setType({
            name: 'ui-select-single-search',
            extends: 'select',
            templateUrl: 'ui-select-single-async-search.html'
        });

        formlyConfig.setType({
            name: 'ui-select-multiple',
            extends: 'select',
            templateUrl: 'ui-select-multiple.html'
        });
        formlyConfig.setType({
            name: 'typhead',
            template: '<input type="text" ng-model="model[options.key]" uib-typeahead="item for item in to.options" class="form-control">',
            wrapper: ['bootstrapLabel', 'bootstrapHasError']
        });
        formlyConfig.setType({
            name: 'ui-select-multiple-async',
            extends: 'select',
            templateUrl: 'ui-select-multiple-async.html'
        });
        formlyConfig.setType({
            name: 'ui-select-multiplex',
            extends: 'select',
            templateUrl: 'ui-select-multiplex.html'
        });
        formlyConfig.setType({
            name: 'ui-select-multiplesample',
            extends: 'select',
            templateUrl: 'ui-select-multiplesample.html'
        });
    })
    .run(function (formlyConfig, formlyValidationMessages) {
        formlyConfig.extras.ngModelAttrsManipulatorPreferBound = true;
        formlyValidationMessages.addStringMessage('maxlength', 'Uzun bir karakter girisi');
        formlyValidationMessages.addStringMessage('required', 'Giris zorunludur.');
        formlyValidationMessages.messages.pattern = function (viewValue, modelValue, scope) {
            return viewValue + ' gecersiz bir deger..';
        };
        formlyValidationMessages.addTemplateOptionValueMessage('pattern', 'patternValidationMessage', '', '', 'gecersiz kayit..');
        formlyValidationMessages.addTemplateOptionValueMessage('minlength', 'minlength', '', 'En kucuk deger', 'Kisa bir giris');
    })
    .run(['$rootScope',
        function ($rootScope) {
            $rootScope.FName = $rootScope.FName;
            $rootScope.Gorevi = $rootScope.Gorevi;
            $rootScope.Img = $rootScope.Img;
            $rootScope.servisen = $rootScope.servisen;
            $rootScope.SB = $rootScope.SB;
        }])
    .run(function (editableOptions) {
        editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
    })
    .run(function (formlyConfig) {
        var attributes = [
          'date-disabled',
          'custom-class',
          'show-weeks',
          'starting-day',
          'init-date',
          'min-mode',
          'max-mode',
          'format-day',
          'format-month',
          'format-year',
          'format-day-header',
          'format-day-title',
          'format-month-title',
          'year-range',
          'shortcut-propagation',
          'datepicker-popup',
          'show-button-bar',
          'current-text',
          'clear-text',
          'close-text',
          'close-on-date-selection',
          'datepicker-append-to-body'
        ];

        var bindings = [
          'datepicker-mode',
          'min-date',
          'max-date'
        ];

        var ngModelAttrs = {};

        angular.forEach(attributes, function (attr) {
            ngModelAttrs[camelize(attr)] = { attribute: attr };
        });

        angular.forEach(bindings, function (binding) {
            ngModelAttrs[camelize(binding)] = { bound: binding };
        });

        formlyConfig.setType({
            name: 'datepicker',
            templateUrl: 'datepicker.html',
            wrapper: ['bootstrapLabel', 'bootstrapHasError'],
            defaultOptions: {
                ngModelAttrs: ngModelAttrs,
                templateOptions: {
                    datepickerOptions: {
                        format: 'MM.dd.yyyy',
                        initDate: new Date()
                    }
                }
            },
            controller: ['$scope', function ($scope) {
                $scope.datepicker = {};

                $scope.datepicker.opened = false;

                $scope.datepicker.open = function ($event) {
                    $scope.datepicker.opened = !$scope.datepicker.opened;
                };
            }]
        });

        function camelize(string) {
            string = string.replace(/[\-_\s]+(.)?/g, function (match, chr) {
                return chr ? chr.toUpperCase() : '';
            });
            // Ensure 1st char is always lowercase
            return string.replace(/^([A-Z])/, function (match, chr) {
                return chr ? chr.toLowerCase() : '';
            });
        }
    })
    .run(['$anchorScroll', function ($anchorScroll) {
        $anchorScroll.yOffset = 45;
    }])
;

