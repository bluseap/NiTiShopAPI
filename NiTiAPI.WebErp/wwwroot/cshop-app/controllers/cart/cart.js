var cartController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();   

    var cachedObj = {
        colors: [],
        sizes: []
    }  

    this.initialize = function () {
        $("#hidParaLoadHeaderCart").val("1");

        $.when(loadColors(),
            loadSizes())
            .then(function () {
                loadCart();
            });

        loadData();
        registerEvents();
    }

    function registerEvents() {

        $('body').on('click', '.btnCartBeginOrder', function (e) {
            e.preventDefault();
            niti.notify("elkrjglk kl", "success");
        });

        $('body').on('click', '.btnContinueShopping', function (e) {
            e.preventDefault();
            var cateId = $('#ddlCategoryId').val();
            var search = $('#txtSearchHeader').val();
            searchProduct(cateId, search);
        });
        
        $('body').on('click', '.btnSearchHeader', function (e) {
            e.preventDefault();
            var cateId = $('#ddlCategoryId').val();
            var search = $('#txtSearchHeader').val();
            searchProduct(cateId, search);                         
        });

        $('#txtSearchHeader').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                var cateId = $('#ddlCategoryId').val();
                var search = $('#txtSearchHeader').val();
                searchProduct(cateId, search);
            }
        });

        $('body').on('click', '.btn-CartDelete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/cshop/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    niti.notify(resources["RemoveCartOK"], 'success');
                    loadHeaderCart();
                    loadCart();
                }
            });
        });

        $('body').on('keyup', '.txtCartQuantity', function (e) {
            e.preventDefault();
            var ddlColor = 0;
            var ddlSizeId = 0;

            var id = $(this).data('id');
            var q = $(this).val();
            if (q > 0) {
                $.ajax({
                    url: '/cshop/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q,
                        color: ddlColor,
                        size: ddlSizeId
                    },
                    success: function () {
                        niti.notify(resources["UpdateCartOK"], 'success');
                        loadHeaderCart();
                        loadCart();
                    }
                });
            } else {
                niti.notify(resources["UpdateCartError"], 'error');
            }
        });

        $('body').on('change', '.ddlColorId', function (e) {
            e.preventDefault();
            var ddlColor = $("#ddlColorId").val();
            var ddlSizeId = 0;

            var id = parseInt($(this).closest('tr').data('id'));
            //var colorId = $(this).val();
            var q = $(this).closest('tr').find('.txtQuantity').first().val();
            //var sizeId = $(this).closest('tr').find('.ddlSizeId').first().val();
            
            $.ajax({
                url: '/cshop/Cart/UpdateCart',
                type: 'post',
                data: {
                    productId: id,
                    quantity: q,
                    color: ddlColor,
                    size: ddlSizeId
                },
                success: function () {
                    niti.notify(resources["UpdateCartOK"], 'success');
                    loadHeaderCart();
                    loadCart();
                },
                error: function () {
                    niti.notify(resources['NotFound'], 'error');
                }
            });            
        });

        $('body').on('change', '.ddlSizeId', function (e) {
            e.preventDefault();
            var ddlColor = 0;
            var ddlSizeId = $("#ddlSizeId").val();

            var id = parseInt($(this).closest('tr').data('id'));
            //var sizeId = $(this).val();
            var q = parseInt($(this).closest('tr').find('.txtQuantity').first().val());
            //var colorId = parseInt($(this).closest('tr').find('.ddlColorId').first().val());
            
            $.ajax({
                url: '/cshop/Cart/UpdateCart',
                type: 'post',
                data: {
                    productId: id,
                    quantity: q,
                    color: ddlColor,
                    size: ddlSizeId
                },
                success: function () {
                    niti.notify(resources["UpdateCartOK"], 'success');
                    loadHeaderCart();
                    loadCart();
                },
                error: function () {
                    niti.notify(resources['NotFound'], 'error');
                }
            });           
        });

        $('#btnClearAll').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/cshop/Cart/ClearCart',
                type: 'post',
                success: function () {
                    niti.notify(resources["ClearCart"] , 'success');
                    loadHeaderCart();
                    loadCart();
                },
                error: function () {
                    niti.notify(resources['NotFound'], 'error');
                }
            });
        });

    }

    function loadData() {
        //loadCart();
        loadCategoty();
    }

    function searchProduct(cateId, search) {
        //clientshop/product/search/nitiapp?catelogyId=0&keyword=0&sortBy=lastest&pageSize=24
        var href = "/cshop/product/search/nitiapp?catelogyId=" + cateId + "&keyword=" + search +
            "&sortBy=lastest&pageSize=12";
        window.open(href, '_parent');
        return false;
    }

    function loadCategoty() {
        return $.ajax({
            type: 'GET',
            url: '/cshop/product/GetListCategory',
            dataType: 'json',
            success: function (response) {
                var choosen = resources["All"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCategoryId').html(render);
                $("#ddlCategoryId")[0].selectedIndex = 0;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/cshop/product/GetListAttribute",
            data: {
                attributeId: 1, // Colors
                language: "vi-VN"
            },
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                niti.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/cshop/product/GetListAttributeAll",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                niti.notify('Has an error in progress', 'error');
            }
        });
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
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
        var sizes = "<select class='form-control ddlSizeId' disabled>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Value + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Value + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function loadCart() {
        $.ajax({
            url: '/cshop/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.ImageUrl,
                            Price: niti.formatNumber(item.Price, 0),
                            Quantity: item.Quantity,
                            Colors: getColorOptions(item.Color === null ? 0 : item.Color.Id),
                            Sizes: getSizeOptions(item.Size === null ? 0 : item.Size.Id),
                            Amount: niti.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lblTotalAmount').text(niti.formatNumber(totalAmount, 0));
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('You have no product in cart');
            }
        });
        return false;
    }

    function loadHeaderCart() {
        $("#headerCart").load("/cshop/AjaxContent/HeaderCart");
    }

    

}