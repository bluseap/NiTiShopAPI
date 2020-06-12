var appuserController = function () {

    var userCorporationId = $("#hidUserCorporationId").val();
    var userName = $("#hidUserName").val();

    var addedituser = new addedituserController();
    var editpass = new editpassController();

    this.initialize = function () {

        loadCorporation();

        loadData();

        registerEvents();

        addedituser.initialize();
        editpass.initialize();        
    }

    function registerEvents() {
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                addedituser.loadTableUser();
            }
        });

        $("#btn-search").on('click', function () {
            addedituser.loadTableUser();
        });

        $("#ddl-show-pageUser").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            addedituser.loadTableUser();
        });

        $("#btn-create").on('click', function () {
            addedituser.AddEditClearData();
            addedituser.initRoleList();
            addedituser.disableAddEdit(false);
            // 1 - Insert User
            $('#hidInsertUser').val(1);
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            addedituser.disableAddEdit(true);
            var userId = $(this).data('id');
            // 2 - Update User
            $('#hidInsertUser').val(2);

            loadEditUser(userId);
        });

        $('body').on('click', '.btn-editpass', function (e) {
            e.preventDefault();
            
            var userId = $(this).data('id');
            $('#hidUserId').val(userId);
            // 2 - Update User
            $('#hidInsertUser').val(2);            
            $('#modal-edit-password').modal('show');           
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var userId = $(this).data('id');

            deleteUser(userId);
        });

    }

    function loadData() {
        addedituser.AddEditClearData();
    }

    function loadCorporation() {
        return $.ajax({
            type: 'GET',
            url: '/admin/Corporation/GetListCorporations',
            dataType: 'json',
            success: function (response) {                
                var render = "<option value='0' >-- Tất cả --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.id + "'>" + item.name + "</option>";
                });
                $('#ddlCorporation').html(render);
                $('#ddlAddUpdateCorporation').html(render);

                if (userCorporationId !== "1") {
                    $('#ddlCorporation').prop('disabled', true);
                    $('#ddlAddUpdateCorporation').prop('disabled', true);
                }
                else {
                    $('#ddlCorporation').prop('disabled', false);
                    $('#ddlAddUpdateCorporation').prop('disabled', false);
                }

                $("#ddlCorporation")[0].selectedIndex = 1;
                $("#ddlAddUpdateCorporation")[0].selectedIndex = 1;

                addedituser.loadTableUser();
            },
            error: function () {
                niti.notify('Không tìm thấy!', 'error');
            }
        });
    }

    function loadEditUser(userid) {
        $.ajax({
            type: "GET",
            url: "/Admin/AppUser/GetListUserRole",
            data: { userId: userid },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidUserId').val(data.id);

                $('#imagelistUserImage').html('');               
                $('#imagelistUserImage').append('<div ><img width="100"  data-path="' + data.avatar + '" src="' + data.avatar + '" /></div>');
                $('#hidAvatarUser').val(data.avatar);

                $('#txtFullName').val(data.fullName);
                $('#txtUserName').val(data.userName);
                $('#txtEmail').val(data.email);
                $('#txtPhoneNumber').val(data.phoneNumber);
                $('#ddlAddUpdateCorporation').val(data.corporationId);
                $('#ckStatus').prop('checked', data.status === 1);               

                addedituser.initRoleList(data.items);

                $('#modal-add-edit').modal('show');
                niti.stopLoading();
            },
            error: function () {
                niti.notify("Không tìm thấy!", 'error');
                niti.stopLoading();
            }
        });
    }

    function deleteUser(userid) {
        niti.confirm(resources["DeleteSure"], function () {
            $.ajax({
                type: "POST",
                url: "/Admin/AppUser/DeleteUser",
                data: {
                    Id: userid,
                    userName: userName
                },
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete User.");
                    niti.notify("Xóa thành công.", 'success');
                    niti.stopLoading();
                    addedituser.loadTableUser();
                },
                error: function (status) {
                    niti.notify('Has an error in deleting progress', 'error');
                    niti.stopLoading();
                }
            });
        });
    }


}