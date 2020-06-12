
var listOrderController = function () {

    var userName = $("#hidUserName").val();
    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.loadTableListOrder = function (orderId) {
        loadTableListOrder(orderId);
    }
    
    this.initialize = function () {     
        loadAttributeSize();
        loadColors();  
        loadAllSizes();
        loadCatalog();

        registerEvents();
        quantitiesClearData();
    }

    function registerEvents() {
        $('#txtPTOKeyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadTableProduct();
            }
        });

        $("#btnPTOSearch").on('click', function () {
            loadTableProduct();
        });

        $("#ddl-show-pageProducts").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            loadTableProduct(true);
        });

        $("#btnSaveListOrder").on('click', function () {
            var insertListOrderQuantites = $('#hidInsertListOrderQuantities').val();
            saveListOrderQuantity();
            //if (insertListOrderQuantites === "1") // insert
            //{
            //    saveListOrderQuantity();
            //}
            //else if (insertListOrderQuantites === "2") // update
            //{
            //    updateListOrderQuantity();
            //}
            //else {
            //    niti.notify(resources["CreateTableError"], "error");
            //}
        });

        $('#btn-add-quantityListOrder').on('click', function () {
            loadTableProduct();
            $('#modal-add-AddProductToOrder').modal('show');            
        });

        $('body').on('click', '.btn-delete-quantityListOrder', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });

        $("#ddlAttributeSize").on('change', function () {
            var attributeId = $("#ddlAttributeSize").val();
            //loadSizes(attributeId);
        });   

        $('body').on('click', '.btn-AddProductToOrder', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            loadEditProduct(productId);            
        });   

        $('body').on('click', '.bntExcelOrder', function (e) {
            e.preventDefault();
            var orderId = $('#hidOrderId').val();
            loadExportExcelOrder(orderId);
        });
       
    }   

    function quantitiesClearData() {
        $('#hidInsertListOrderQuantities').val(0);
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId' >";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Value + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Value + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId' id='ddlSelectSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Value + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Value + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function loadAttributeSize() {
        return $.ajax({
            type: 'GET',
            url: '/admin/product/GetAttributeSize',
            data: {
                codeSize: "kich-co",
                languageId: "vi-VN"
            },
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlAttributeSize').html(render);
                $("#ddlAttributeSize")[0].selectedIndex = 0;

            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Admin/AttributeOptionValue/GetListAttribute",
            data: {
                attributeId: 1, // Colors
                language: "vi-VN"
            },
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadSizes(attributeId) {
        return $.ajax({
            type: "GET",
            url: "/Admin/AttributeOptionValue/GetListAttribute",
            data: {
                attributeId: attributeId, // Sizes
                language: "vi-VN"
            },
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }    

    function loadAllSizes() {
        return $.ajax({
            type: "GET",
            url: "/admin/order/GetListAttriSizes",
            data: {
                codeSize: 'kich-co',
                language: 'vi-VN'
            },
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                niti.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadTableListOrder(orderid) {
        $.ajax({
            url: '/admin/order/GetListOrderDetail',
            data: {
                orderId: orderid,
                languageId: "vi-VN"
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.length === 0) {
                    $('#hidInsertListOrderQuantities').val(1); // insert
                }
                else {
                    $('#hidInsertListOrderQuantities').val(2); // update                   
                }

                loadOrderId(orderid);

                var render = '';
                var template = $('#template-table-orderDetails').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.OrderId,
                        ProductId: item.ProductId,
                        ProductName: item.ProductName,
                        Colors: getColorOptions(item.AttributeOptionValueIdColor),
                        Sizes: getSizeOptions(item.AttributeOptionValueIdSize),
                        Quantity: item.Quantity
                    });
                });
                $('#table-quantity-orderDetails').html(render);
            }
        });
    }

    function loadOrderId(orderid) {
        $.ajax({
            type: "GET",
            url: "/Admin/order/GetId",
            data: {
                id: orderid
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var order = response;

                $('#hidOrderId').val(order.Id);

                $('#txtListOrderCustomerName').val(order.CustomerName);    
               
                niti.stopLoading();
            },
            error: function (status) {
                niti.notify(status, 'error');
                niti.stopLoading();
            }
        });
    }

    function loadTableProduct(isPageChanged) {
        var corporationid = $('#ddlPTOCorporation').val();
        var catalogid = $('#ddlPTOCategorySearch').val();

        $.ajax({
            type: "GET",
            url: "/admin/product/GetPaging",
            data: {
                keyword: $('#txtPTOKeyword').val(),
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
                $('#ddlPTOCategorySearch').html(render);
                $("#ddlPTOCategorySearch")[0].selectedIndex = 0;
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
                var orderId = $('#hidOrderId').val();
                var render = '';
                var template = $('#template-table-orderDetails').html();
                render = Mustache.render(template, {
                    Id: orderId,
                    ProductName: product.Name,
                    Colors: getColorOptions(null),
                    Sizes: getSizeOptions(null),
                    Quantity: 0,
                    ProductId: productid
                });
                $('#table-quantity-orderDetails').append(render);

                $('#modal-add-AddProductToOrder').modal('hide'); 
                
                niti.stopLoading();
            },
            error: function (status) {
                niti.notify(status, 'error');
                niti.stopLoading();
            }
        });
    }

    function saveListOrderQuantity() {
        var orderList = [];
        $.each($('#table-quantity-orderDetails').find('tr'), function (i, item) {
            orderList.push({
                Id: $(item).data('id'),              
                Colors: $(item).find('select.ddlColorId').first().val(),
                Sizes: $(item).find('select.ddlSizeId').first().val(),
                Quantity: $(item).find('input.txtQuantity').first().val(),
                ProductId: $(item).find('input.hiddenProductId').first().val()             
            });
        });

        var xml = '';
        xml = xml + "<tables>";
        for (var i = 0; i < orderList.length; i++) {
            var listfield = orderList[i];
            xml += "<items>";
            xml += '<Id>' + listfield.Id + '</Id>';
            xml += '<ColorId>' + listfield.Colors + '</ColorId>';
            xml += '<SizeId>' + listfield.Sizes + '</SizeId>';
            xml += '<Quantity>' + listfield.Quantity + '</Quantity>';  
            xml += '<ProductId>' + listfield.ProductId + '</ProductId>';
            xml += "</items>";
        }
        xml = xml + '</tables>';
        //alert(xml);
        $.ajax({
            type: "POST",
            url: '/admin/order/SaveListOrder',
            data: {
                listOrderXML: xml,
                username: userName,
                languageId: "vi-VN"
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function () {
                niti.appUserLoginLogger(userName, "Update or delete Order Details.");
                niti.stopLoading();
                
                $('#modal-add-editListOrder').modal('hide');
                //$('#table-quantity-content').html('');
            }
        });

    }

    function loadExportExcelOrder(orderid) {                
        $.ajax({
            type: 'POST',
            url: '/admin/order/ExportExcel',
            data: {
                orderId: orderid
            },
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                window.location.href = response;
                niti.stopLoading();
            }
        });
    }

    function updateListOrderQuantity() {

    }

}