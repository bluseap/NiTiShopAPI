
var addeditproductController = function () {

    var userName = $("#hidUserName").val();
    var imageProduct = [];

    this.loadTableProduct = function (isPageChanged) {
        loadTableProduct(isPageChanged);
    }

    this.AddEditClearData = function () {
        AddEditClearData();
    }

    this.loadProductCatelogies = function (selectedId) {
        loadProductCatelog(selectedId);
    }

    this.initialize = function () {
        loadAddEditData();
        registerEvents();

    }   

    function registerEvents() {

        formMainValidate();

        $('#btnAddEditSave').on('click', function (e) {
            var insertProduct = $('#hidInsertProduct').val();
            if (insertProduct === "1") {
                saveProduct(e);
            }
            else if (insertProduct === "2") {
                updateProduct(e);
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }
        });   

        registerControls();

        $("#fileInputImageProduct").on('change', function () {
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
                    clearfileImageProduct($("#fileInputImageProduct"));
                    imageProduct.push(path);

                    $('#imagelistImageProduct').append('<div ><img width="100"  data-path="' + path + '" src="' + path + '"></div>');
                    niti.notify(resources["UploadFile"], 'success');
                },
                error: function () {
                    niti.notify(resources["UploadFileError"], 'error');
                }
            });
        });

    }

    function registerControls() {
        CKEDITOR.replace('txtAddEditContent', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function loadAddEditData() {
        loadProductCatelog();
        loadUnits();
    }

    function formMainValidate() {
        jQuery.validator.addMethod("isCompobox", function (value, element) {
            if (value === "%" || value === "0" || value === "")
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
        $('#frmMaintainanceAddEdit').validate({
            errorClass: 'red',
            ignore: [],
            language: 'vi',
            rules: {
                ddlAddEditCategories: {
                    isCompobox: true
                },
                txtAddEditName: { required: true },
                ddlAddEditUnit: {
                    isCompobox: true
                },
                txtSalePrice: { number: true },
                txtOriginalPrice: { number: true },
                txtPromotionPrice: { number: true }                
            }
        });
    }

    function loadProductCatelog(selectedId) {
        $.ajax({
            url: "/Admin/Catalog/GetListCategory",
            type: 'GET',
            dataType: 'json',
            async: false,
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
                var arr = niti.unflattern(data);
                $('#ddlAddEditCategories').combotree({
                    data: arr
                });

                //$('#ddlCategoryIdImportExcel').combotree({
                //    data: arr
                //});
                if (selectedId !== undefined) {
                    $('#ddlAddEditCategories').combotree('setValue', selectedId);
                }
            }
        });
    }

    function loadUnits() {
        return $.ajax({
            type: 'GET',
            url: '/admin/AttributeOptionValue/GetListAttribute',
            data: {
                attributeId: "3", // Units
                language: "vi-VN"
            },
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Value + "</option>";
                });
                $('#ddlAddEditUnit').html(render); 
                $("#ddlAddEditUnit")[0].selectedIndex = 1;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function AddEditClearData() {
        $('#ddlAddEditCorporation')[0].selectedIndex = 1;

        loadProductCatelog("");

        $('#txtAddEditName').val("");
        $('#txtAddEditDescription').val("");
        $('#ddlAddEditUnit')[0].selectedIndex = 1;
        $('#txtSalePrice').val(0);
        $('#txtOriginalPrice').val(0);
        $('#txtPromotionPrice').val(0);

        clearfileImageProduct($("#fileInputImageProduct"));
        imageProduct = [];

        $('#txtSeoTitle').val("");
        $('#txtSeoAlias').val("");
        $('#txtSeoKeyword').val("");
        $('#txtSeoDescription').val("");
        $('#txtSeoTag').val("");

        $('#ckAddEditActive').prop('checked', true);
        $('#ckAddEditHot').prop('checked', true);
        $('#ckAddEditHome').prop('checked', true);
    }

    function loadTableProduct(isPageChanged) {
        var corporationid = $('#ddlCorporation').val();
        var catalogid = $('#ddlCategorySearch').val();

        $.ajax({
            type: "GET",
            url: "/admin/product/GetPaging",
            data: {
                keyword: $('#txtKeyword').val(),
                corporationId: corporationid,
                categoryId: catalogid,
                pageIndex: niti.configs.pageIndex,
                pageSize: niti.configs.pageSize,
                culture: "vi-VN"
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var template = $('#table-templateProduct').html();
                var render = "";
                if (response.Items.length > 0) {
                    $.each(response.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Name: item.Name,
                            CategoryName: item.CategoryName,
                            CorporationName: item.CorporationName,
                            Price: niti.formatNumber(item.Price, '.'),
                            ImageUrl: item.ImageUrl === null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.ImageUrl + '" width=25 />',
                            Status: niti.getStatus(item.Status)
                        });
                    });

                    $("#lblTotalRecords").text(response.TotalRow);
                    if (render !== undefined) {
                        $('#tbl-contentProduct').html(render);
                    }

                    if (response.TotalRow !== 0) {
                        wrapPagingProduct(response.TotalRow, function () {
                            loadTableProduct();
                        },
                            isPageChanged);
                    }
                }
                else {
                    $('#tbl-contentProduct').html('');
                }
                niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }
    function wrapPagingProduct(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / niti.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationULProduct a').length === 0 || changePageSize === true) {
            $('#paginationULProduct').empty();
            $('#paginationULProduct').removeData("twbs-pagination");
            $('#paginationULProduct').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationULProduct').twbsPagination({
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

    function clearfileImageProduct(ctrl) {
        try {
            imageProduct = [];
            ctrl.value = null;
            ctrl.value('');
        }
        catch (status) {
            console.log(status);
        }
    }

    function saveProduct(e) {
        e.preventDefault();        
        
        if ($('#frmMaintainanceAddEdit').valid()) {          
            var corporationId = $('#ddlAddEditCorporation').val();
            var categoryId = $('#ddlAddEditCategories').combotree('getValue');
            var name = $('#txtAddEditName').val();
            var description = $('#txtAddEditDescription').val();
            var unit = $('#ddlAddEditUnit').val();
            var price = $('#txtSalePrice').val();
            var originalPrice = $('#txtOriginalPrice').val();
            var promotionPrice = $('#txtPromotionPrice').val();

            //imageProduct

            var content = CKEDITOR.instances.txtAddEditContent.getData();
            var seoTitle = $('#txtSeoTitle').val();
            var seoAlias = $('#txtSeoAlias').val();
            var seoKeyword = $('#txtSeoKeyword').val();
            var seoDescripts = $('#txtSeoDescription').val();
            var seoTags = $('#txtSeoTag').val();  
            
            var activeProduct = $('#ckAddEditActive').prop('checked') === true ? true : false;
            var hotProduct = $('#ckAddEditHot').prop('checked') === true ? true : false;
            var homeProduct = $('#ckAddEditHome').prop('checked') === true ? true : false;

            $.ajax({
                type: "POST",
                url: "/Admin/Product/CreateProduct",
                data: { 
                    CorporationId: corporationId,
                    CategoryId: categoryId,
                    Name: name,
                    Description: description,
                    AttributeValueText: unit,
                    Price: price,
                    OriginalPrice: originalPrice,
                    DiscountPrice: promotionPrice,
                    ImageUrl: imageProduct,
                    Contents: content,
                    SeoTitle: seoTitle,
                    SeoAlias: seoAlias,
                    SeoKeyword: seoKeyword,
                    SeoDescription: seoDescripts,
                    SeoTags: seoTags,
                    IsActive: activeProduct,
                    HotFlag: hotProduct,
                    HomeFlag: homeProduct,
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Save Product.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTableProduct(true);
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }   

    }

    function updateProduct(e) {
        e.preventDefault();

        if ($('#frmMaintainanceAddEdit').valid()) {
            var productId = $('#hidProductId').val();
            var corporationId = $('#ddlAddEditCorporation').val();
            var categoryId = $('#ddlAddEditCategories').combotree('getValue');
            var name = $('#txtAddEditName').val();
            var description = $('#txtAddEditDescription').val();
            var unit = $('#ddlAddEditUnit').val();
            var price = $('#txtSalePrice').val();
            var originalPrice = $('#txtOriginalPrice').val();
            var promotionPrice = $('#txtPromotionPrice').val();

            var hidimageProduct = $('#hidImageProduct').val();

            var content = CKEDITOR.instances.txtAddEditContent.getData();
            var seoTitle = $('#txtSeoTitle').val();
            var seoAlias = $('#txtSeoAlias').val();
            var seoKeyword = $('#txtSeoKeyword').val();
            var seoDescripts = $('#txtSeoDescription').val();
            var seoTags = $('#txtSeoTag').val();

            var activeProduct = $('#ckAddEditActive').prop('checked') === true ? true : false;
            var hotProduct = $('#ckAddEditHot').prop('checked') === true ? true : false;
            var homeProduct = $('#ckAddEditHome').prop('checked') === true ? true : false;

            $.ajax({
                type: "POST",
                url: "/Admin/Product/UpdateProduct",
                data: {
                    Id: productId,
                    CorporationId: corporationId,
                    CategoryId: categoryId,
                    Name: name,
                    Description: description,
                    AttributeValueText: unit,
                    Price: price,
                    OriginalPrice: originalPrice,
                    DiscountPrice: promotionPrice,
                    ImageUrl: imageProduct.length > 0 ? imageProduct : hidimageProduct,
                    Contents: content,
                    SeoTitle: seoTitle,
                    SeoAlias: seoAlias,
                    SeoKeyword: seoKeyword,
                    SeoDescription: seoDescripts,
                    SeoTags: seoTags,
                    IsActive: activeProduct,
                    HotFlag: hotProduct,
                    HomeFlag: homeProduct,
                    UpdateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Update Product.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTableProduct(true);
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }   

    }

}