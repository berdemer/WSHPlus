function intersect() {
    return function (arr1, arr2) {
        return arr1.filter(function (n) {
            return arr2.indexOf(n) !== -1
        });
    };
};

function degerYok() {
    return function (text) {
        if (text == null) {
            return "Kayıtsız!";
        }
        return text;
    }
};

function resimYok() {
    return function (arguman) {
        if (arguman == "") {
            return "40aa38a9-c74d-4990-b301-e7f18ff83b08.png";
        } else
            return arguman;
    }
};

function inArray($filter) {
    return function (list, arrayFilter, element) {
        if (arrayFilter) {
            return $filter("filter")(list, function (listItem) {
                return arrayFilter.indexOf(listItem[element]) != -1;
            });
        }
    };
};

function getAge() {
    return function (birthDate) {
        if (birthDate === undefined) { return 'Geçersiz!'; } else {
            var today = new Date();
            var birth = new Date(birthDate);
            var age = today.getFullYear() - birth.getFullYear();
            var ageMonth = today.getMonth() - birth.getMonth();
            if (ageMonth < 0 || (ageMonth === 0 && today.getDate() < birth.getDate())) { age-- };
            return age;
        }
    }
}

function propsFilter() {//select tipi sorgulamada (person in rols | propsFilter: {FullName: $select.search, Meslek: $select.search})
    return function (items, props) {
        var out = [];
        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;
                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }
                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }
        return out;
    }
};

function miktarDuzeni() {
    return function (text) {
        if (text === null) {
            return "Miktar kaydı girilmedi!";
        }
        return text;
    }
};

function kullanimDuzeni() {
    return function (text) {
        if (text === null) {
            return "Kullanım şekli girilmedi.";
        }
        return text;
    }
};

function sayiYaz() {
    return function (num) {
        var a = ['', 'Bir ', 'İki ', 'Üç ', 'Dört ', 'Beş ', 'Altı ', 'Yedi ', 'Sekiz ', 'Dokuz '];
        var b = ['', 'On', 'Yirmi', 'Otuz', 'Kırk', 'Elli', 'Altmış', 'Yetmiş', 'Seksen', 'Doksan'];
        if ((num = num.toString()).length > 9) return 'hata:rakamda taşma!';
        n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
        if (!n) return; var str = '';
        str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Milyar ' : '';
        str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Milyon ' : '';
        str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Bin ' : '';
        str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Yüz ' : '';
        str += (n[5] != 0) ? ((str != '') ? '' : '') + (a[Number(n[5])] || b[n[5][0]] + '' + a[n[5][1]]) : '';
        return str;
    }
};


function romanRakamiYaz() {
    return function (num) {
        var lookup = {
            M: 1000, CM: 900, D: 500, CD: 400, C: 100, XC: 90,
            L: 50, XL: 40, X: 10, IX: 9, V: 5, IV: 4, I: 1
        },
            roman = '',
            i;
        for (i in lookup) {
            while (num >= lookup[i]) {
                roman += i;
                num -= lookup[i];
            }
        }
        return ' D(' + roman + ')B  ';
    }
};

function romanRakami() {
    return function (num) {
        num = num + 1;
        var lookup = {
            M: 1000, CM: 900, D: 500, CD: 400, C: 100, XC: 90,
            L: 50, XL: 40, X: 10, IX: 9, V: 5, IV: 4, I: 1
        },
            roman = '',
            i;
        for (i in lookup) {
            while (num >= lookup[i]) {
                roman += i;
                num -= lookup[i];
            }
        }
        return roman + '. ';
    };
}

function trim() {
    return function (val) {
        if (!angular.isString(val)) {
            return val;
        }
        return val.replace(/^\s+|\s+$/g, '');
    };
}

function kullanimSekli() {
    return function (valx) {
        var liste = [
            {
                "verilis": "Ağızdan(Oral)",
                "val": 1
            },
            {
                "verilis": "Intra müsküler(IM)",
                "val": 9
            },
            {
                "verilis": "Cilt üzerine(Epidermal)",
                "val": 2
            },
            {
                "verilis": "Intra venöz(IV)",
                "val": 15
            },
            {
                "verilis": "inhalasyon",
                "val": 17
            },
            {
                "verilis": "Ağız içi",
                "val": 4
            },
            {
                "verilis": "Göz üzerine",
                "val": 10
            },
            {
                "verilis": "Burun içi(Intranazal)",
                "val": 5
            },
            {
                "verilis": "Dış kulak yolu",
                "val": 7
            },
            {
                "verilis": "Dil altı(Sublingual)",
                "val": 6
            },
            {
                "verilis": "Subkutan",
                "val": 14
            },
            {
                "verilis": "Solunum yolu",
                "val": 3
            },
            {
                "verilis": "Kolon",
                "val": 8
            },
            {
                "verilis": "Rektal",
                "val": 11
            },
            {
                "verilis": "Intra vajinal",
                "val": 12
            },
            {
                "verilis": "Intra dermal",
                "val": 13
            },
            {
                "verilis": "Kalp içi",
                "val": 16
            },
            {
                "verilis": "Trans dermal",
                "val": 18
            },
            {
                "verilis": "İntravezikal",
                "val": 19
            },
            {
                "verilis": "İntra-artiküler",
                "val": 20
            },
            {
                "verilis": "İntraperitoneal",
                "val": 21
            },
            {
                "verilis": "İntravitreal",
                "val": 22
            },
            {
                "verilis": "İntratekal",
                "val": 23
            },
            {
                "verilis": "İntraligamenter",
                "val": 24
            },
            {
                "verilis": "Perinöral",
                "val": 25
            },
            {
                "verilis": "İntrakaviter",
                "val": "26"
            },
            {
                "verilis": "",
                "val": 99
            }
        ];

        var y = liste.filter(function (number) {
            return number.val === valx;
        });
        return y[0].verilis;
    };
}

function kullanimPeriyod() {
    return function (valx) {
        var liste = [
            {
                adi: "Günde",
                val: 3
            },
            {
                adi: "Haftada",
                val: 4
            },
            {
                adi: "Ayda",
                val: 5
            },
            {
                adi: "Yılda",
                val: 6
            }
        ];
        var y = liste.filter(function (number) {
            return number.val === valx;
        });
        return y[0].adi;
    };
}


function numToTime() {
    return function (num) {
        var h = Math.floor(num / 60);
        var m = num % 60;
        h = h < 10 ? '0' + h : h;
        m = m < 10 ? '0' + m : m;
        return h + ':' + m;
    };
}

function reverse() {
        return function (items) {
            return items.slice().reverse();
        };
}


function tel() {
    return function (phonenum) {
        return phonenum !== null ? "0 (" + phonenum.substr(0, 3) + ") " +
            phonenum.substr(3, 3) + " " + phonenum.substr(6, 2) + " " +
            phonenum.substr(8, 2) : "Veri Yok";
    };
}


angular
    .module('inspinia')
    .filter('intersect', intersect)
    .filter('degerYok', degerYok)
    .filter('resimYok', resimYok)
    .filter('inArray', inArray)
    .filter('getAge', getAge)
    .filter('propsFilter', propsFilter)
    .filter('miktarDuzeni', miktarDuzeni)
    .filter('kullanimDuzeni', kullanimDuzeni)
    .filter('sayiYaz', sayiYaz)
    .filter('romanRakamiYaz', romanRakamiYaz)
    .filter('romanRakami', romanRakami)
    .filter('trim', trim)
    .filter('kullanimSekli', kullanimSekli)
    .filter('kullanimPeriyod', kullanimPeriyod)
    .filter('numToTime', numToTime)
    .filter('reverse', reverse)
    .filter('tel', tel)
    ;