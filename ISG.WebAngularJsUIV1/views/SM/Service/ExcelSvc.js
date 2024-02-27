'use strict';

function ExcelSvc() {
	var ExcelSvcFactory = {};
	var _xlsxSave = function (jsonTable, ExcName, ExcSheetName, columnNonList) {
		var RegxDate0 = new RegExp('\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}[-\\+]\\d{2}:\\d{2}');
		var RegxDate1 = new RegExp('\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}');
		var RegxDate2 = new RegExp('\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}Z');
		var RegxDate3 = new RegExp('\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}[-\\+]\\d{2}:?\\d{2}');
		var data = [];
		var baslik = [];
		$.each(jsonTable, function (key, entry) {
			var tasi = [];
			baslik = [];
			var i = 0;
			$.each(entry, function (key, e) {
				if (columnNonList.indexOf(i) == -1) {
					baslik.push(key);
					e = e == null ? '  ' : e;
					var regxSonucu = RegxDate0.test(e) || RegxDate1.test(e) || RegxDate2.test(e) || RegxDate3.test(e);
					tasi.push(regxSonucu && !Number.isFinite(e) ? new Date(e) : e);
				}
				i++;
			});
			data.push(tasi);
		});
		data.unshift(baslik);
		function datenum(v, date1904) {
			if (date1904) v += 1462;
			var epoch = Date.parse(v);
			return (epoch - new Date(Date.UTC(1899, 11, 30))) / (24 * 60 * 60 * 1000);
		}
		function sheet_from_array_of_arrays(data, opts) {
			var ws = {};
			var range = { s: { c: 10000000, r: 10000000 }, e: { c: 0, r: 0 } };
			for (var R = 0; R != data.length; ++R) {
				for (var C = 0; C != data[R].length; ++C) {
					if (range.s.r > R) range.s.r = R;
					if (range.s.c > C) range.s.c = C;
					if (range.e.r < R) range.e.r = R;
					if (range.e.c < C) range.e.c = C;
					var cell = { v: data[R][C] };
					if (cell.v == null) continue;
					var cell_ref = XLSX.utils.encode_cell({ c: C, r: R });

					if (typeof cell.v === 'number') cell.t = 'n';
					else if (typeof cell.v === 'boolean') cell.t = 'b';
					else if (cell.v instanceof Date) {
						cell.t = 'n'; cell.z = XLSX.SSF._table[14];
						cell.v = datenum(cell.v);
					}
					else cell.t = 's';

					ws[cell_ref] = cell;
				}
			}
			if (range.s.c < 10000000) ws['!ref'] = XLSX.utils.encode_range(range);
			return ws;
		}
		var ws_name = ExcSheetName;
		function Workbook() {
			if (!(this instanceof Workbook)) return new Workbook();
			this.SheetNames = [];
			this.Sheets = {};
		}
		var wb = new Workbook(), ws = sheet_from_array_of_arrays(data);
		wb.SheetNames.push(ws_name);
		wb.Sheets[ws_name] = ws;
		var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });
		function s2ab(s) {
			var buf = new ArrayBuffer(s.length);
			var view = new Uint8Array(buf);
			for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
			return buf;
		}
		saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), ExcName);
	};

	ExcelSvcFactory.XlsxSave = _xlsxSave;
	return ExcelSvcFactory;
}
angular
	.module('inspinia')
	.factory('ExcelSvc', ExcelSvc);