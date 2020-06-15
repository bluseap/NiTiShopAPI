
var addedituserController = function () {
   
    var userName = $("#hidUserName").val();

    var imageUser = [];

    this.initRoleList = function (selectedRoles) {
        initRoleList(selectedRoles);
    }

    this.loadTableUser = function (isPageChanged) {
        loadTableUser(isPageChanged);
    }

    this.AddEditClearData = function () {
        AddEditClearData();        
    }

    this.disableAddEdit = function (is) {
        disableAddEdit(is);
    }
   
    this.initialize = function () {
        registerEvents();        
    }

    function registerEvents() { 
        
        formMainValidate();
                
        $('#btnSaveUser').on('click', function (e) {
            var insertUser = $('#hidInsertUser').val();
            if (insertUser === "1") {
                saveUser(e);
            }
            else if (insertUser === "2") {
                updateUser(e);
            }
            else {
                niti.notify("Thêm mớ lỗi!", "error");
            }            
        });   

        $("#fileInputUserImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();

            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImageUser",
                //url: "/Admin/Upload/UploadImageResizeW60H90",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearfileUserImage($("#fileInputUserImage"));
                    imageUser.push(path);

                    $('#imagelistUserImage').append('<div ><img width="100"  data-path="' + path + '" src="' + path + '"></div>');
                    niti.notify("Upload file thành công", 'success');
                },
                error: function () {
                    niti.notify("Upload bị lỗi", 'error');
                }
            });
        });
    }

    function AddEditClearData() {
        $('#hidUserId').val('');
        $('#hidInsertUser').val(0);
        $('#hidAvatarUser').val("");

        initRoleList();

        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ddlAddUpdateCorporation')[0].selectedIndex = 1;
        $('#ckStatus').prop('checked', true);   

        clearfileUserImage($("#fileInputUserImage"));
        imageUser = [];
    }

    function disableAddEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);
    }

    function formMainValidate() {
        jQuery.validator.addMethod("isCompobox", function (value, element) {
            if (value === "%" || value === "0")
                return false;
            else
                return true;
        },
            "Chọn !"
        );

        jQuery.validator.addMethod("isDateVietNam", function (value, element) {
            return this.optional(element) || moment(value, "DD/MM/YYYY").isValid();
        },
            resources["DD/MM/YYYY"]
        );

        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            language: 'vi',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: { required: true },
                txtConfirmPassword: { required: true },
                txtEmail: { required: true },
                txtPhoneNumber: { required: true },
                ddlAddUpdateCorporation: {
                    isCompobox: true
                }              
            }           
        });
    }

    function loadTableUser(isPageChanged) {
        var corporation = $('#ddlCorporation').val();

        $.ajax({
            type: "GET",
            url: "/admin/appuser/GetPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                cororationId: corporation,
                pageIndex: niti.configs.pageIndex,
                pageSize: niti.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var template = $('#table-templateUser').html();
                var render = "";
                if (response.items.length > 0) {
                    $.each(response.items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.id,
                            UserName: item.userName,                            
                            FullName: item.fullName,
                            CorporationName: item.corporationName,
                            Avatar: item.avatar !== null ? '<img src="' + item.avatar + '?h=60" />' :
                                '<img src="/admin-side/images/img.jpg?h=60" />' ,                           
                            Status: niti.getStatus(item.status)
                        });
                    });

                    $("#lbl-total-recordsUser").text(response.totalRow);
                    if (render !== undefined) {
                        $('#tbl-contentUser').html(render);
                    }

                    if (response.totalRow !== 0) {
                        wrapPagingUser(response.totalRow, function () {
                            loadTableUser();
                        },
                            isPageChanged);
                    }
                }
                else {
                    $('#tbl-contentUser').html('');
                }
                niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }
    function wrapPagingUser(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / niti.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationULUser a').length === 0 || changePageSize === true) {
            $('#paginationULUser').empty();
            $('#paginationULUser').removeData("twbs-pagination");
            $('#paginationULUser').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationULUser').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '< ',
            prev: '<< ',
            next: '>> ',
            last: '> ',
            onPageClick: function (event, p) {
                if (niti.configs.pageIndex !== p) {
                    niti.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            }
        });
    }

    function initRoleList(selectedRoles) {
        var addeditCorporationId = $("#ddlAddUpdateCorporation").val();

        $.ajax({
            type: 'GET',
            url: "/Admin/AppRole/GetCoporationId",
            data: { cororationId: addeditCorporationId },
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response.items;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';

                    if (selectedRoles !== undefined) { // selectedRoles.indexOf(item.Name) !== -1) {
                        $.each(selectedRoles, function (j, jtem) {
                            if (jtem.name === item.name) {
                                checked = 'checked';
                            }
                        });                        
                    }

                    render += Mustache.render(template,
                        {
                            Id: item.id,
                            Name: item.name,
                            Description: item.description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });        
    }
    
    function clearfileUserImage(ctrl) {
        try {
            imageUser = [];
            ctrl.value = null;
            ctrl.value('');
        }
        catch (status) {
            console.log(status);
        }
    }

    function saveUser(e) {
        e.preventDefault();
        if ($('#frmMaintainance').valid()) {          

            var fullName = $('#txtFullName').val();          
            var username = $('#txtUserName').val();
            var password = $('#txtPassword').val();
            var email = $('#txtEmail').val();
            var phoneNumber = $('#txtPhoneNumber').val();            
         
            var status = $('#ckStatus').prop('checked') === true ? 1 : 0;
            var corporationId = $('#ddlCorporation').val();

            $.ajax({
                type: "POST",
                url: "/Admin/AppUser/SaveUser",
                data: {             
                    Avatar: imageUser,
                    FullName: fullName,                 
                    UserName: username,
                    PasswordHash: password,
                    Email: email,
                    PhoneNumber: phoneNumber,
                    CorporationId: corporationId,                    
                    Status: status,
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Save User and Roles.");
                    niti.notify("Thêm mới thành công", 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTableUser(true);
                },
                error: function () {
                    niti.notify("Thêm mới lỗi!", 'error');
                    niti.stopLoading();
                }
            });
        }       
    }

    function updateUser(e) {
        e.preventDefault();
        if ($('#frmMaintainance').valid()) {
            var userid = $('#hidUserId').val();
            var avartarUser = $('#hidAvatarUser').val();

            var fullName = $('#txtFullName').val();            
            var email = $('#txtEmail').val();
            var phoneNumber = $('#txtPhoneNumber').val();
            var roles = [];
            $.each($('input[name="ckRoles"]'), function (i, item) {
                if ($(item).prop('checked') === true)
                    roles.push($(item).prop('value'));
            });

            var userroles = [];
            $.each(roles, function (i, item) {
                userroles += item + ',';
            });

            var status = $('#ckStatus').prop('checked') === true ? true : false;
            var corporationId = $('#ddlCorporation').val();

            $.ajax({
                type: "POST",
                url: "/Admin/AppUser/UpdateUser",
                data: {
                    Id: userid,
                    Avatar: imageUser.length > 0 ? imageUser : avartarUser,
                    FullName: fullName,                                     
                    Email: email,
                    PhoneNumber: phoneNumber,
                    CorporationId: corporationId,
                    Active: status,
                    UpdateBy: userName,
                    Roles: userroles     
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Update User and Roles.");
                    niti.notify("Sửa thành công.", 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTableUser(true);
                },
                error: function () {
                    niti.notify("Sửa lỗi.", 'error');
                    niti.stopLoading();
                }
            });
        }  
    }

}