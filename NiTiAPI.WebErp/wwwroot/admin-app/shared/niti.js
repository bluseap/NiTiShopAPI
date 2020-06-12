var niti = {
    configs: {
        pageSize: 10,
        pageIndex: 1
    },
    notify: function (message, type) {
        $.notify(message, {
            // whether to hide the notification on click
            clickToHide: true,
            // whether to auto-hide the notification
            autoHide: true,
            // if autoHide, hide after milliseconds
            autoHideDelay: 5000,
            // show the arrow pointing at the element
            arrowShow: true,
            // arrow size in pixels
            arrowSize: 5,
            // position defines the notification position though uses the defaults below
            position: '...',
            // default positions
            elementPosition: 'top right',
            globalPosition: 'top right',
            // default style
            style: 'bootstrap',
            // default class (string or [string])
            className: type,
            // show animation
            showAnimation: 'slideDown',
            // show animation duration
            showDuration: 400,
            // hide animation
            hideAnimation: 'slideUp',
            // hide animation duration
            hideDuration: 200,
            // padding between element and notification
            gap: 2
        });
    },
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Cance',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
            }
        });
    },
    confirmOk: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Cance',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
                else {
                    okCallback();
                }
            }
        });
    },

    getFormattedDateTimeHour: function (datetime) {
        if (datetime === null || datetime === '')
            return '';

        var newdate = new Date(datetime);

        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        var ss = newdate.getSeconds();

        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        //return day + "/" + month + "/" + year;
        //var str = newdate.getFullYear() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getDate() +
        //    " " + newdate.getHours() + ":" + newdate.getMinutes() + ":" + newdate.getSeconds();

        return day + month + year + hh + mm + ss;
    },

    getFormattedDate: function (datetime) {
        if (datetime === null || datetime === '')
            return '';

        var newdate = new Date(datetime);

        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        return day + "/" + month + "/" + year;

        //var str = newdate.getFullYear() + "-" + (newdate.getMonth() + 1) + "-" + newdate.getDate() +
        //    " " + newdate.getHours() + ":" + newdate.getMinutes() + ":" + newdate.getSeconds();

        //return day + "/" + month + "/" + year;
    },

    getFormattedDateTimeN: function (datetime) {
        if (datetime === null || datetime === '')
            return '';

        var newdate = new Date(datetime);

        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();

        var hh = newdate.getHours();
        var mm = newdate.getMinutes();

        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        return day + "/" + month + "/" + year + "    " + hh + ":" + mm;
       
    },

    getFormattedDateYYYYMMDD: function (datetime) {
        if (datetime === null || datetime === '')
            return '';

        var newdate = new Date(datetime);

        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();

        var hh = newdate.getHours();
        var mm = newdate.getMinutes();

        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        //return day + "/" + month + "/" + year + "    " + hh + ":" + mm;
        return year + month + day;
    },

    getFormattedDateYYYYMM: function (datetime) {
        if (datetime === null || datetime === '')
            return '';

        var newdate = new Date(datetime);

        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();

        var hh = newdate.getHours();
        var mm = newdate.getMinutes();

        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        //return day + "/" + month + "/" + year + "    " + hh + ":" + mm;
        return year + " / " + month;

    },

    getFormatDateYYMMDD: function (datetime) {    
        var ngaysinh = datetime.split("/");
        var f = new Date(ngaysinh[2], ngaysinh[1] - 1, ngaysinh[0]).toDateString("yyyy/MM/dd");

        return f;
    },    

    dateFormatJson: function (datetime) {
        if (datetime === null || datetime === '')
            return '';
        var newdate = new Date(parseInt(datetime.substr(6)));
        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        return day + "/" + month + "/" + year;
    },
    dateTimeFormatJson: function (datetime) {
        if (datetime === null || datetime === '')
            return '';
        var newdate = new Date(parseInt(datetime.substr(6)));
        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        var ss = newdate.getSeconds();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        if (ss < 10)
            ss = "0" + ss;
        return day + "/" + month + "/" + year + " " + hh + ":" + mm + ":" + ss;
    },
    startLoading: function () {
        //if ($('.dv-loading').length > 0)
        //    $('.dv-loading').removeClass('hide');        
        $('body').loading({
            stoppable: true,
            message: 'Loading...',
            theme: 'dark'
        });
    },
    stopLoading: function () {
        //if ($('.dv-loading').length > 0)
        //    $('.dv-loading').addClass('hide');
        $(':loading').loading('stop');
    },
    getGoiTinh: function (gioitinh) {
        if (gioitinh === '1')
            return '<span class="badge bg-blue">Nam</span>';
        else if (gioitinh === '0')
            return '<span class="badge bg-green">Nữ</span>';
        else 
            return '<span > </span>';
    },
    getStatus: function (status) {
        if (status === 1)
            return '<span class="badge bg-green">Còn hàng</span>';
        else
            return '<span class="badge bg-red">Hết hàng</span>';
    },
    getStatusPosts: function (status) {
        if (status === 1)
            return '<span class="badge bg-green">Active</span>';
        else
            return '<span class="badge bg-red">Inactive</span>';
    },

    getHoSoNhanVienStatus: function (status) {
        if (status === 1)
            return '<span class="badge bg-green">Làm việc</span>';
        else if (status === 2)
            return '<span class="badge bg-red">Nghĩ việc</span>';
        else if (status === 3)
            return '<span class="badge bg-blue">Về hưu</span>';
        else if (status === 4)
            return '<span class="badge bg-orange">Hết hạn</span>';

        else if (status === 30)
            return '<span class="badge bg-green">Đang học</span>';

        else if (status === 41)
            return '<span class="badge bg-green">Online</span>';
        else if (status === 42)
            return '<span class="badge bg-red">Offline</span>'; 
        else if (status === 43)
            return '<span class="badge bg-orange">Chưa đầy</span>';   
        else if (status === 44)
            return '<span class="badge bg-blue">Tủ đầy</span>';   

        
        else if (status === 61)
            return '<span class="badge bg-green">Mới</span>';
        else if (status === 62)
            return '<span class="badge bg-red">Cũ</span>';
        else if (status === 60)
            return '';

        else if (status === 71)
            return '<span class="badge bg-green">Hoạt động</span>';    
        else 
            return '<span class="badge bg-purple">Chưa biết</span>';
    },
    getVanBanDenTTXuLy: function (trangthai) {
        if (trangthai === "1")
            return '<span class="badge bg-orange">Chưa chuyển</span>';
        else if (trangthai === "2")
            return '<span class="badge bg-blue">Đã chuyển</span>';
        else if (trangthai === "3")
            return '<span class="badge bg-green">Đã duyệt</span>';       
        else if (trangthai === "4")
            return '<span class="badge bg-purple">Phát hành</span>';
        else
            return '<span class="badge bg-red">Không biết</span>';
    },
    getVanBanDenTTDuyet: function (trangthai) {
        if (trangthai === 1)
            return '<span class="badge bg-green">Chuyển chuyên môn</span>';
        else if (trangthai === 2)
            return '<span class="badge bg-orange">Chưa chuyển chuyên môn</span>';
        else if (trangthai === 3)
            return '<span class="badge bg-red">Sai chuyển lại</span>';
        else if (trangthai === 4)
            return '<span class="badge bg-blue">Duyệt văn thư</span>';
        else
            return '<span class="badge bg-red">Không biết</span>';
    },
    getVanBanDenTTDangXuLy: function (trangthai) {
        if (trangthai === 1)
            return '<span class="badge bg-orange">Nhập văn bản đến</span>';
        else if (trangthai === 2)
            return '<span class="badge bg-green">Chuyển lãnh đạo</span>';
        else if (trangthai === 3)
            return '<span class="badge bg-blue">Lãnh đạo duyệt</span>';
        else if (trangthai === 4)
            return '<span class="badge bg-purple">Kết thúc</span>';
        else if (trangthai === 5)
            return '<span class="badge bg-red">Xử lý lại</span>';
        else
            return '<span class="badge bg-red">Không biết</span>';
    },
    getVanBanDiTTChoXuLy: function (trangthai) {
        if (trangthai === 1)
            return '<span class="badge bg-orange">Đang xử lý</span>';
        else if (trangthai === 2)
            return '<span class="badge bg-green">Chuyển lãnh đạo</span>';
        else if (trangthai === 3)
            return '<span class="badge bg-blue">Đã duyệt</span>';
        //else if (trangthai === "4")
        //    return '<span class="badge bg-blue">Chuyển văn thư</span>';
        else if (trangthai === 5)
            return '<span class="badge bg-red">Xử lý lại</span>';
        else if (trangthai === 0)
            return '<span class="badge bg-purple">Phát hành</span>';
        else
            return '<span class="badge bg-purple">Không biết</span>';
    },
    getVanBanDiTTChoDuyet: function (trangthai) {
        if (trangthai === 1)
            return '<span class="badge bg-orange">Chờ duyệt</span>';
        //else if (trangthai === "2")
        //    return '<span class="badge bg-green">Chuyển lãnh đạo</span>';
        else if (trangthai === 3)
            return '<span class="badge bg-red">Sai trả về</span>';
        else if (trangthai === 4)
            return '<span class="badge bg-blue">Chuyển văn thư</span>';
        //else if (trangthai === "5")
        //    return '<span class="badge bg-red">Xử lý lại</span>';
        else if (trangthai === 0)
            return '<span class="badge bg-purple">Phát hành</span>';
        else
            return '<span class="badge bg-purple">Không biết</span>';
    },
    getVanBanDiTTChuaPhatHanh: function (trangthai) {
        if (trangthai === 1)
            return '<span class="badge bg-orange">Chưa phát hành</span>';
        //else if (trangthai === "2")
        //    return '<span class="badge bg-green">Chuyển lãnh đạo</span>';
        //else if (trangthai === "3")
        //    return '<span class="badge bg-blue">Lãnh đạo duyệt</span>';
        else if (trangthai === 4)
            return '<span class="badge bg-purple">Phát hành</span>';
        //else if (trangthai === "5")
        //    return '<span class="badge bg-red">Xử lý lại</span>';

        else if (trangthai === 0)
            return '<span class="badge bg-purple">Phát hành</span>';
        else
            return '<span class="badge bg-red">Không biết</span>';
    },

    formatNumber: function (number, precision) {
        if (!isFinite(number)) {
            return number.toString();
        }
        var a = number.toFixed(precision).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
        return a.join('.');
    },
    formatNumberKhongLe: function (number2) {
        //var number = 330000.22;
        var number = parseInt(number2);
        if (!isFinite(number)) {
            return number.toString();
        }

        var a = number.toFixed(0).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
        return a.join('.');
    },
    
    unflattern: function (arr) {
        var map = {};
        var roots = [];
        for (var i = 0; i < arr.length; i += 1) {
            var node = arr[i];
            node.children = [];
            map[node.id] = i; // use map to look-up the parents
            if (node.parentId !== null && node.parentId !== 0) {
                arr[map[node.parentId]].children.push(node);
            } else {
                roots.push(node);
            }
        }
        return roots;
    },

    isVanBanDen: function(isvanbanden) {
        return $.ajax({
            type: 'GET',
            url: '/admin/homevanban/IsVanBanDen',
            data: {
                isVanBanDen: isvanbanden
            },
            dataType: 'json',
            success: function (response) {
                var ketqua = response.Success;
                if (ketqua === false) {
                    window.location.href = "/homevanban/index";
                    tedu.notify("Không đủ quyền. Kiểm tra lại!", "error");
                }
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Phân quyền có vấn đề?', 'error');
            }
        });
    },    

    isVanBanDi: function (isvanbandi) {
        return $.ajax({
            type: 'GET',
            url: '/admin/homevanban/IsVanBanDi',
            data: {
                isVanBanDi: isvanbandi
            },
            dataType: 'json',
            success: function (response) {
                var ketqua = response.Success;
                if (ketqua === false) {
                    window.location.href = "/homevanban/index";
                    tedu.notify("Không đủ quyền. Kiểm tra lại!", "error");
                }
            },
            error: function (status) {
                console.log(status);
                niti.notify('Phân quyền có vấn đề?', 'error');
            }
        });
    },
    
    appUserLoginLogger: function (username, stattuscontent) {
        return $.ajax({
            type: 'GET',
            url: '/admin/AppUserLogin/CountUserLogin',
            data: {
                userNameId: username,
                statuscontent: stattuscontent
            },
            dateType: 'json',           
            success: function (res) {
            },
            error: function (status) {
                console.log(status);
                niti.notify('User logger.', 'error');
            }
        });
    }

}

$(document).ajaxSend(function(e, xhr, options) {
    if (options.type.toUpperCase() === "POST" || options.type.toUpperCase() === "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();
        xhr.setRequestHeader("RequestVerificationToken", token);
    }
});
