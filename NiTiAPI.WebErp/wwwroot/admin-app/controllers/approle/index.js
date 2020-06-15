var approleController = function () {
 
    var userCorporationId = $("#hidUserCorporationId").val();     
    var userName = $("#hidUserName").val(); 

    var addeditRole = new addeditroleController();
    var permission = new permissionController();

    this.initialize = function () {     
      
        loadCorporation();

        loadData();

        registerEvents();

        addeditRole.initialize();
        permission.initialize();
        //loadFunctionList();
    }

    function registerEvents() {

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                addeditRole.loadTableRole();
            }
        });

        $("#btn-search").on('click', function () {
            addeditRole.loadTableRole();
        });

        $("#ddl-show-pageRoles").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            addeditRole.loadTableRole(true);
        });

        $("#btn-create").on('click', function () {
            addeditRole.clearAddEditData();
            // 1 - Update Role
            $('#hidInsertRole').val(1);

            $('#modal-add-edit').modal('show');
        });       

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var roleId = $(this).data('id');    
            // 2 - Update Role
            $('#hidInsertRole').val(2);
            loadEditRole(roleId);            
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();            
            var roleId = $(this).data('id');
            deleteRole(roleId);            
        });
       
        //Grant permission
        $('body').on('click', '.btn-grant', function (e) {           
            e.preventDefault();
            var roleId = $(this).data('id');    

            $('#hidRoleId').val(roleId);               
          
            permission.fillPermission(roleId);           

            $('#modal-grantpermission').modal('show');
        });  
        
    };   
       
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

                addeditRole.loadTableRole();
            },
            error: function () {                
                niti.notify("Không tìm thấy", 'error');
            }
        });
    }

    function loadEditRole(roleid) {
        $.ajax({
            type: "GET",
            url: "/Admin/AppRole/GetRoleId",
            data: { id: roleid },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var role = response;

                $('#hidRoleId').val(role.id);               

                $('#ddlAddUpdateCorporation').val(role.corporationId);
                $('#txtRoleName').val(role.name);
                $('#txtRoleDescription').val(role.description);

                $('#modal-add-edit').modal('show');
                niti.stopLoading();
            },
            error: function (status) {
                niti.notify(status, 'error');
                niti.stopLoading();
            }
        });
    }

    function deleteRole(roleId) {
        niti.confirm("Chắc chắn xóa?", function () {
            $.ajax({
                type: "POST",
                url: "/Admin/AppRole/DeleteRole",
                data: {
                    Id: roleId,
                    userName: userName
                },
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete Role.");
                    niti.notify("Xóa thành công.", 'success');
                    niti.stopLoading();
                    addeditRole.loadTableRole();
                },
                error: function (status) {
                    niti.notify('Has an error in deleting progress', 'error');
                    niti.stopLoading();
                }
            });
        });
    }

    function loadData() {

    }

}