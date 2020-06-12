var catalogController = function () {

    var userCorporationId = $("#hidUserCorporationId").val();
    var userName = $("#hidUserName").val();

    var addeditcatalog = new addeditcatalogController();   

    this.initialize = function () {

        loadCorporation();

        loadData();

        registerEvents();

        addeditcatalog.initialize();
        
    }

    function registerEvents() {

        $("#btnCreate").on('click', function () {       
            addeditcatalog.AddEditClearData();
            // 1 - Insert 
            $('#hidInsertCatalog').val(1);
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '#btnCatalogEdit', function (e) {
            e.preventDefault();
            var catalogId = $('#hidCatalogId').val();
            // 2 - Update 
            $('#hidInsertCatalog').val(2);
            loadEditCatalog(catalogId);
        });

        $('body').on('click', '#btnCatalogDelete', function (e) {
            e.preventDefault();
            var catalogId = $('#hidCatalogId').val();
            deleteCatalog(catalogId);
        });

    }

    function loadCorporation() {
        return $.ajax({
            type: 'GET',
            url: '/admin/Corporation/GetListCorporations',
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCorporation').html(render);
                $('#ddlAddEditCorporation').html(render);

                if (userCorporationId !== "1") {
                    $('#ddlCorporation').prop('disabled', true);
                    $('#ddlAddEditCorporation').prop('disabled', true);
                }
                else {
                    $('#ddlCorporation').prop('disabled', false);
                    $('#ddlAddEditCorporation').prop('disabled', false);
                }

                $("#ddlCorporation")[0].selectedIndex = 1;
                $("#ddlAddEditCorporation")[0].selectedIndex = 1;

                addeditcatalog.loadListCatalog();
                addeditcatalog.loadCatalogParent($("#ddlCorporation").val());
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadData() {
        addeditcatalog.AddEditClearData();
    }

    function loadEditCatalog(catalogid) {
        $.ajax({
            type: "GET",
            url: "/Admin/catalog/GetCategoryId",
            data: { Id: catalogid },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var data = response;
                
                $('#hidCatalogId').val(data.Id);

                $('#txtCatalogName').val(data.Name);
                $('#ddlCatalogParent').val(data.ParentId);
                $('#txtCatalogDescription').val(data.Description);
                $('#txtCatalogSortOrder').val(data.SortOrder);
                $('#txtSeoTitle').val(data.SeoTitle);
                $('#txtSeoAlias').val(data.SeoAlias);
                $('#txtSeoKeyword').val(data.SeoKeyword);
                $('#txtSeoDescription').val(data.SeoDescription);        

                addeditcatalog.loadListCatalog();

                $('#modal-add-edit').modal('show');
                niti.stopLoading();
            },
            error: function () {
                niti.notify(resources["NotFound"], 'error');
                niti.stopLoading();
            }
        });
    }

    function deleteCatalog(catalogid) {
        niti.confirm(resources["DeleteSure"], function () {
            $.ajax({
                type: "POST",
                url: "/Admin/catalog/Delete",
                data: {
                    Id: catalogid,
                    username: userName
                },
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete User.");
                    niti.notify(resources["DeleteTableOK"], 'success');
                    niti.stopLoading();
                    addeditcatalog.loadListCatalog();
                },
                error: function (status) {
                    niti.notify('Has an error in deleting progress', 'error');
                    niti.stopLoading();
                }
            });
        });
    }

}