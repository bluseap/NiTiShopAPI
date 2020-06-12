
var addeditCategoryNewsController = function () {

    var userName = $("#hidUserName").val();

    this.loadListCatalog = function () {
        loadListCatalog();
    }

    this.AddEditClearData = function () {
        AddEditClearData();
    }

    this.initialize = function () {
        registerEvents();
    }

    this.loadCatalogParent = function (corporationId) {
        loadCatalogParent(corporationId);
    }

    function registerEvents() {

        formMainValidate();

        $('#btnEditCatalogSave').on('click', function (e) {
            var insert = $('#hidInsertCatalog').val();
            if (insert === "1") {
                saveCatalog(e);
            }
            else if (insert === "2") {
                updateCatalog(e);
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }
        });

    }

    function loadCatalogParent(corporationid) {
        return $.ajax({
            type: 'GET',
            url: '/admin/CategoryNews/GetCoporationId',
            data: { cororationId: corporationid },
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response.Items, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCatalogParent').html(render);
                //$("#ddlCatalogParent")[0].selectedIndex = 1;                            
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadListCatalog() {
        $.ajax({
            url: '/Admin/CategoryNews/GetListCategory',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });

                });
                var treeArr = niti.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $('#treeProductCategory');

                $('#treeCatalog').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        $('#hidCatalogId').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        //console.log(target);
                        //console.log(source);
                        //console.log(point);
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });                           

                            //Update to database
                            $.ajax({
                                url: '/Admin/catalog/UpdateParent',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    fromParent: source.id,
                                    toParent: targetNode.id,
                                    parameter: 1,
                                    username: userName
                                },
                                success: function (res) {
                                    niti.appUserLoginLogger(userName, "Update Catalog.");
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {                           

                            $.ajax({
                                url: '/Admin/catalog/UpdateParent',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    fromParent: source.id,
                                    toParent: targetNode.id,
                                    parameter: 0,
                                    username: userName
                                },
                                success: function (res) {
                                    niti.appUserLoginLogger(userName, "Update Catalog.");
                                }
                            });
                        }
                    }
                });
            }
        });

    }

    function AddEditClearData() {
        $('#hidCatalogId').val("");
        $('#hidInsertCatalog').val(0);

        $('#txtCatalogName').val("");
        $('#ddlCatalogParent')[0].selectedIndex = 0;
        $('#txtCatalogDescription').val("");
        $('#txtCatalogSortOrder').val(1);
        $('#txtSeoTitle').val("");
        $('#txtSeoAlias').val("");
        $('#txtSeoKeyword').val("");
        $('#txtSeoDescription').val("");

        $('#ckShowInMenu').prop('checked', true);
        $('#ckShowInHome').prop('checked', false);
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
                txtCatalogName: { required: true },
                ddlCatalogParent: {
                    isCompobox: true
                },
                ddlAddEditCorporation: {
                    isCompobox: true
                },
                txtCatalogDescription: { required: true },
                txtCatalogSortOrder: { required: true },
                txtSeoTitle: { required: true },
                txtSeoAlias: { required: true },
                txtSeoKeyword: { required: true },
                txtSeoDescription: { required: true }
            }
        });
    }

    function saveCatalog(e) {
        e.preventDefault();
        if ($('#frmMaintainance').valid()) {

            var name = $('#txtCatalogName').val();
            var parent = $('#ddlCatalogParent').val();
            var corporation = $('#ddlAddEditCorporation').val();
            var descripts = $('#txtCatalogDescription').val();
            var sortorder = $('#txtCatalogSortOrder').val();
            var seotitle = $('#txtSeoTitle').val();
            var seoalias = $('#txtSeoAlias').val();
            var seokey = $('#txtSeoKeyword').val();
            var seodesctipt = $('#txtSeoDescription').val();

            var showInMenu = $('#ckShowInMenu').prop('checked') === true ? true : false;
            var showInHome = $('#ckShowInHome').prop('checked') === true ? true : false;

            $.ajax({
                type: "POST",
                url: "/Admin/CategoryNews/Create",
                data: {
                    Name: name,
                    ParentId: parent,
                    CorporationId: corporation,
                    Description: descripts,
                    SortOrder: sortorder,
                    SeoTitle: seotitle,
                    SeoAlias: seoalias,
                    SeoKeyword: seokey,
                    SeoDescription: seodesctipt,
                    ShowInMenu: showInMenu,
                    ShowInHome: showInHome,
                    Thumbnail: "",
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Save Category News.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadListCatalog();
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }
    }

    function updateCatalog(e) {
        e.preventDefault();
        if ($('#frmMaintainance').valid()) {
            var catalogId = $('#hidCatalogId').val();
            var name = $('#txtCatalogName').val();
            var parent = $('#ddlCatalogParent').val();
            var corporation = $('#ddlAddEditCorporation').val();
            var descripts = $('#txtCatalogDescription').val();
            var sortorder = $('#txtCatalogSortOrder').val();
            var seotitle = $('#txtSeoTitle').val();
            var seoalias = $('#txtSeoAlias').val();
            var seokey = $('#txtSeoKeyword').val();
            var seodesctipt = $('#txtSeoDescription').val();

            var showInMenu = $('#ckShowInMenu').prop('checked') === true ? true : false;
            var showInHome = $('#ckShowInHome').prop('checked') === true ? true : false;

            $.ajax({
                type: "POST",
                url: "/Admin/CategoryNews/Update",
                data: {
                    Id: catalogId,
                    Name: name,
                    ParentId: parent,
                    CorporationId: corporation,
                    Description: descripts,
                    SortOrder: sortorder,
                    SeoTitle: seotitle,
                    SeoAlias: seoalias,
                    SeoKeyword: seokey,
                    SeoDescription: seodesctipt,
                    ShowInMenu: showInMenu,
                    ShowInHome: showInHome,
                    Thumbnail: "",
                    UpdateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Update Category News.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadListCatalog();
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }
    }

}