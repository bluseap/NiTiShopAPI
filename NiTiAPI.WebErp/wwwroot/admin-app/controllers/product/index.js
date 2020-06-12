var productController = function () {

    var userCorporationId = $("#hidUserCorporationId").val();
    var userName = $("#hidUserName").val();

    var addeditproduct = new addeditproductController();
    var imageproduct = new imageproductController();
    var importexcel = new importexcelController();
    var quantityProduct = new quantityController();
    var wholeprice = new wholepriceController();

    this.initialize = function () {

        loadCorporation();
        loadData();
        registerEvents();

        addeditproduct.initialize();
        imageproduct.initialize();
        importexcel.initialize();
        quantityProduct.initialize();
        wholeprice.initialize();
    }

    function registerEvents() {

        $('#txtKeyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                addeditproduct.loadTableProduct();
            }
        });

        $("#btnSearch").on('click', function () {
            addeditproduct.loadTableProduct();
        });

        $("#ddl-show-pageProducts").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            addeditproduct.loadTableProduct(true);
        });

        $("#btnCreate").on('click', function () {
            addeditproduct.AddEditClearData();
            // 1 - insert Product
            $('#hidInsertProduct').val(1);

            $('#modal-add-edit').modal('show');
        });   

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            // 2 - Update Product
            $('#hidInsertProduct').val(2);
            loadEditProduct(productId);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            deleteProduct(productId);
        });

        $('body').on('click', '.btn-images', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            $('#hidProductId').val(productId);
            imageproduct.loadImageProduct(productId);
            $('#modal-image-manage').modal('show');
        });

        $('body').on('click', '.btn-quantity', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            $('#hidProductId').val(productId);
            quantityProduct.loadQuantityProduct(productId);
            $('#modal-quantity-management').modal('show');
        });

        $('body').on('click', '.btn-whole-price', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            $('#hidProductId').val(productId);
            wholeprice.loadWholePrice(productId);
            $('#modal-whole-price').modal('show');
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

                addeditproduct.loadTableProduct();               
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadData() {
        addeditproduct.AddEditClearData();
        loadCatalog();
    }

    function loadCatalog() {
        return $.ajax({
            type: 'GET',
            url: '/admin/Catalog/GetListCategory',            
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCategorySearch').html(render);
                $("#ddlCategorySearch")[0].selectedIndex = 0;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadEditProduct(productid) {
        $.ajax({
            type: "GET",
            url: "/Admin/product/GetProductId",
            data: {
                id: productid,
                culture: "vi-VN"
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var product = response;

                $('#hidProductId').val(product.Id);

                $('#ddlAddEditCorporation').val(product.CorporationId);
                
                addeditproduct.loadProductCatelogies(product.CategoryId);

                $('#txtAddEditName').val(product.Name);
                $('#txtAddEditDescription').val(product.Description);
                $('#ddlAddEditUnit').val(product.AttributeValueText);
                $('#txtSalePrice').val(product.Price);
                $('#txtOriginalPrice').val(product.OriginalPrice);
                $('#txtPromotionPrice').val(product.DiscountPrice);

                $('#imagelistImageProduct').html('');               
                $('#imagelistImageProduct').append('<div ><img width="100"  data-path="' + product.ImageUrl + '" src="' + product.ImageUrl + '" /></div>');
                $('#hidImageProduct').val(product.ImageUrl);

                CKEDITOR.instances.txtAddEditContent.setData(product.Contents);
                $('#txtSeoTitle').val(product.SeoTitle);
                $('#txtSeoAlias').val(product.SeoAlias);
                $('#txtSeoKeyword').val(product.SeoKeyword);
                $('#txtSeoDescription').val(product.SeoDescription);
                $('#txtSeoTag').val(product.SeoTags);
                $('#ckAddEditActive').prop('checked', product.IsActive);
                $('#ckAddEditHot').prop('checked', product.HotFlag);
                $('#ckAddEditHome').prop('checked', product.HomeFlag);

                $('#modal-add-edit').modal('show');
                niti.stopLoading();
            },
            error: function (status) {
                niti.notify(status, 'error');
                niti.stopLoading();
            }
        });
    }

    function deleteProduct(productid) {
        niti.confirm(resources["DeleteSure"], function () {
            $.ajax({
                type: "POST",
                url: "/Admin/product/DeleteProduct",
                data: {
                    Id: productid,
                    username: userName
                },
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete Product.");
                    niti.notify(resources["DeleteTableOK"], 'success');
                    niti.stopLoading();
                    addeditproduct.loadTableProduct();
                },
                error: function (status) {
                    niti.notify('Has an error in deleting progress', 'error');
                    niti.stopLoading();
                }
            });
        });
    }

}