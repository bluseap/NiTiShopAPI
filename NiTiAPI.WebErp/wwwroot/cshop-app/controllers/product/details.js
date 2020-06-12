var ProductDetailController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();  

    var coporationNameId = $("#hidCorporationNameId").val();
    var productId = $("#hidProductId").val();

    var colorId = "0";
    var sizeId = "0";

    this.initialize = function () {
        registerEvents();
        loadData();        
    }

    function registerEvents() {

        $("#ddlSelectColor").on('change', function () {
            var colorProduct = $("#ddlSelectColor").val();
            colorId = colorProduct;

            var classColor = "0";
            if (colorProduct === "14") {
                classColor = "colorBack";
            }
            else if (colorProduct === "1") {
                classColor = "colorRed";
            }
            else if (colorProduct === "15") {
                classColor = "colorOrgane";
            }
            else if (colorProduct === "16") {
                classColor = "colorGreen";
            }
            else if (colorProduct === "17") {
                classColor = "colorBlue";
            }
            else if (colorProduct === "7") {
                classColor = "colorWhile";
            }
            else if (colorProduct === "2") {
                classColor = "colorYellow";
            }
            else if (colorProduct === "8") {
                classColor = "colorBrow";
            }
            else if (colorProduct === "22") {
                classColor = "colorPink";
            }
            else {
                classColor = "0";
            }

            var html = "<h2 class='saider-bar-title'>" + resources['Color']
                + "</h2><ul><li><a class='" + classColor + "' ></a></li></ul>";
            $("#ulliColor").html(html);
        });

        $("#ddlSelectSize").on('change', function () {
            var sizeProduct = $("#ddlSelectSize").val();
            sizeId = sizeProduct;

            var html = "<h2 class='saider-bar-title'>" + resources['Size']
                + "</h2><ul><li><a class='sizeProduct'>" + $("#ddlSelectSize option:selected").text() + "</a></li></ul>";
            $("#ulliSize").html(html);
        });

        $('body').on('click', '.btnSearchHeader', function (e) {
            e.preventDefault();
            var cateId = $('#ddlCategoryId').val();
            var search = $('#txtSearchHeader').val();
            searchProduct(cateId, search);
            //alert(cateId + '.' + search + '.' + corname);                 
        });

        $('#txtSearchHeader').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                var cateId = $('#ddlCategoryId').val();
                var search = $('#txtSearchHeader').val();
                searchProduct(cateId, search);
            }
        });

        $('#btnAddToCart').on('click', function (e) {
            e.preventDefault();
            //var id = parseInt($(this).data('id'));
            var quantity = parseInt($('#txtQuantity').val());            
            //alert(productId + ',' + quantity + ',' + colorId + ',' + sizeId);

            if (productId === "0" || quantity === 0 || colorId === "0" || sizeId === "0") {
                niti.notify(resources['PleaseSelectColor'], 'error');               
            }
            else {               
                $.ajax({
                    url: '/cshop/Cart/AddToCart',
                    type: 'post',                   
                    data: {
                        productId: productId,
                        quantity: parseInt($('#txtQuantity').val()),
                        color: colorId,
                        size: sizeId
                    },
                    success: function () {
                        niti.notify('Product was added successful', 'success');
                        loadHeaderCart();
                        clearDetailData();

                        var href = "/cshop/cart/index/" + coporationNameId ;
                        window.open(href, '_parent');
                    }
                });
            }
        });

        //$('body').on('click', '.colorBack', function (e) {
        //    e.preventDefault();
        //    //alert('den');              
        //    colorId = 14;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Color']
        //        + "</h2><ul><li><a class='colorBack' value=14 ></a></li></ul>";
        //    $("#ulliColor").html(html);
        //});
        //$('body').on('click', '.colorRed', function (e) {
        //    e.preventDefault();
        //    colorId = 1;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Color']
        //        + "</h2><ul><li><a class='colorRed' value=1 ></a></li></ul>";
        //    $("#ulliColor").html(html);
        //});
        //$('body').on('click', '.colorOrange', function (e) {
        //    e.preventDefault();
        //    colorId = 15;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Color']
        //        + "</h2><ul><li><a class='colorOrgane' value=15 ></a></li></ul>";
        //    $("#ulliColor").html(html);
        //});
        //$('body').on('click', '.colorGreen', function (e) {
        //    e.preventDefault();
        //    colorId = 16;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Color']
        //        + "</h2><ul><li><a class='colorGreen' value=16 ></a></li></ul>";
        //    $("#ulliColor").html(html);
        //});
        //$('body').on('click', '.colorBlue', function (e) {
        //    e.preventDefault();
        //    colorId = 17;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Color']
        //        + "</h2><ul><li><a class='colorBlue' value=17 ></a></li></ul>";
        //    $("#ulliColor").html(html);
        //});
        //$('body').on('click', '.colorWhite', function (e) {
        //    e.preventDefault();
        //    colorId = 7;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Color']
        //        + "</h2><ul><li><a class='colorWhile' value=7 ></a></li></ul>";
        //    $("#ulliColor").html(html);
        //});

        //$('body').on('click', '.sizeS', function (e) {
        //    e.preventDefault();    
        //    sizeId = 18;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Size']
        //        + "</h2><ul><li><a class='sizeProduct'>S</a></li></ul>";
        //    $("#ulliSize").html(html);
        //});
        //$('body').on('click', '.sizeL', function (e) {
        //    e.preventDefault();
        //    sizeId = 19;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Size']
        //        + "</h2><ul><li><a class='sizeProduct'>L</a></li></ul>";
        //    $("#ulliSize").html(html);
        //});
        //$('body').on('click', '.sizeM', function (e) {
        //    e.preventDefault();
        //    sizeId = 20;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Size']
        //        + "</h2><ul><li><a class='sizeProduct'>M</a></li></ul>";
        //    $("#ulliSize").html(html);
        //});
        //$('body').on('click', '.sizeXL', function (e) {
        //    e.preventDefault();
        //    sizeId = 3;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Size']
        //        + "</h2><ul><li><a class='sizeProduct'>XL</a></li></ul>";
        //    $("#ulliSize").html(html);
        //});
        //$('body').on('click', '.sizeXXL', function (e) {
        //    e.preventDefault();
        //    sizeId = 21;
        //    var html = "<h2 class='saider-bar-title'>" + resources['Size']
        //        + "</h2><ul><li><a class='sizeProduct'>XXL</a></li></ul>";
        //    $("#ulliSize").html(html);
        //});
       

    }

    function searchProduct(cateId, search) {
        //clientshop/product/search/nitiapp?catelogyId=0&keyword=0&sortBy=lastest&pageSize=24
        var href = "/cshop/product/search/nitiapp?catelogyId=" + cateId + "&keyword=" + search +
            "&sortBy=lastest&pageSize=12";
        window.open(href, '_parent');
        return false;
    }    

    function loadData() {
        loadCategoty();
        loadColors();
        loadSizes(productId);
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
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Value + "</option>";
                });
                $('#ddlSelectColor').html(render);
                $("#ddlSelectColor")[0].selectedIndex = 0;
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadSizes(productid) {
        return $.ajax({
            url: '/cshop/product/GetListAttributeSize',
            data: {
                productId: productid,
                language: "vi-VN"
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Value + "</option>";
                });
                $('#ddlSelectSize').html(render);
                $("#ddlSelectSize")[0].selectedIndex = 0;
            }
        });        
    }

    function clearDetailData() {
        productId = "0";
        colorId = "0";
        sizeId = "0";
    }

    function loadHeaderCart() {
        $("#headerCart").load("/cshop/AjaxContent/HeaderCart");
    }

}