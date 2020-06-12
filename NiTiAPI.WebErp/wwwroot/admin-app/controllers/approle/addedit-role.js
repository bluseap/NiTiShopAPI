
var addeditroleController = function () {    

    var userName = $("#hidUserName").val(); 

    this.loadTableRole = function (isPageChanged) {
        loadTableRole(isPageChanged);
    }

    this.clearAddEditData = function () {
        clearAddEditData();
    }

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        
        //$('#txtNgayBanHanh, #txtNgayDi, #txtHanTraLoiVanBan ').datepicker({
        //    autoclose: true,
        //    format: 'dd/mm/yyyy',
        //    language: 'vi'
        //});

        formMainValidate();

        $('#btnSave').on('click', function (e) {
            var insertRole = $('#hidInsertRole').val();
            if (insertRole === "1") {
                saveRole(e);
            }
            else if (insertRole === "2") {
                updateRole(e);
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }
        });

    }

    function isFormMainValidate() {
        if ($('#frmMaintainance').valid()) {
            return true;
        }
        else {
            return false;
        }
    }
    function formMainValidate() {
        jQuery.validator.addMethod("isCompobox", function (value, element) {
            if (value === "%" || value === "0")
                return false;
            else
                return true;
        },
            resources["PleaseSelect"]
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
                ddlAddUpdateCorporation: {                
                    isCompobox: true
                },
                txtRoleName: { required: true },     
                txtRoleDescription: { required: true }

                //txtNgayDi: {
                //    required: true,
                //    isDateVietNam: true
                //},                
            }
            //    ,
            //messages: {
            //    txtTrichYeu99: { required: "Nhập nội dung trích yếu của văn bản.." }
            //}
        });
    }

    function loadTableRole(isPageChanged) {
        var corporation = $('#ddlCorporation').val();

        $.ajax({
            type: "GET",
            url: "/admin/approle/GetPaging",
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
                var template = $('#table-templateRoles').html();
                var render = "";
                if (response.Items.length > 0) {
                    $.each(response.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Name: item.Name,                            
                            CorporationName: item.CorporationName
                        });
                    });

                    $("#lbl-total-recordsRoles").text(response.TotalRow);
                    if (render !== undefined) {
                        $('#tbl-contentRoles').html(render);
                    }

                    if (response.TotalRow !== 0) {
                        wrapPagingRole(response.TotalRow, function () {
                            loadTableRole();
                        },
                            isPageChanged);
                    }                   
                }
                else {
                    $('#tbl-contentRoles').html('');
                }
                niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }
    function wrapPagingRole(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / niti.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationULRoles a').length === 0 || changePageSize === true) {
            $('#paginationULRoles').empty();
            $('#paginationULRoles').removeData("twbs-pagination");
            $('#paginationULRoles').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationULRoles').twbsPagination({
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

    function clearAddEditData() {
        $('#hidRoleId').val('');
        $('#hidInsertRole').val(0);

        $('#txtRoleName').val('');
        $('#txtRoleDescription').val('');
    }

    function saveRole(e) {     
        e.preventDefault();

        if ($('#frmMaintainance').valid()) {
            //var id = $('#hidRoleId').val();
            var corporation = $('#ddlAddUpdateCorporation').val();
            var name = $('#txtRoleName').val();
            var description = $('#txtRoleDescription').val();

            $.ajax({
                type: "POST",
                url: "/Admin/AppRole/CreateRole",
                data: {
                    CorporationId: corporation,
                    Name: name,
                    Description: description,
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    if (response.Success === false) {
                        niti.notify(resources["CreateTableError"], 'error');
                    }
                    else {
                        niti.appUserLoginLogger(userName, "Create Role.");
                        niti.notify(resources["CreateTableOK"], 'success');
                        $('#modal-add-edit').modal('hide');
                        clearAddEditData();
                        niti.stopLoading();
                        loadTableRole(true);
                    }
                },
                error: function (status) {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        } 
    }

    function updateRole(e) {
        e.preventDefault();

        if ($('#frmMaintainance').valid()) {            
            var id = $('#hidRoleId').val();
            var corporation = $('#ddlAddUpdateCorporation').val();
            var name = $('#txtRoleName').val();
            var description = $('#txtRoleDescription').val();

            $.ajax({
                type: "POST",
                url: "/Admin/AppRole/UpdateRole",
                data: {
                    Id: id,
                    CorporationId: corporation,
                    Name: name,
                    Description: description,
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    if (response.Success === false) {
                        niti.notify(resources["CreateTableError"], 'error');
                    }
                    else {
                        niti.appUserLoginLogger(userName, "Update Role.");
                        niti.notify(resources["CreateTableOK"], 'success');
                        $('#modal-add-edit').modal('hide');
                        clearAddEditData();
                        niti.stopLoading();
                        loadTableRole(true);
                    }
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
            return false;
        }
    }

}