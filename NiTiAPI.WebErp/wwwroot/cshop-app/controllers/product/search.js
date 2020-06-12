var ProductSearchController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();   

    this.initialize = function () {
        registerEvents();
        loadData();
    }

    function registerEvents() {
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

        //$('#btnAddToCart').on('click', function (e) {
        //    e.preventDefault();
        //    var id = parseInt($(this).data('id'));
        //    var colorId = parseInt($('#ddlColorId').val());
        //    var sizeId = parseInt($('#ddlSizeId').val());
        //    $.ajax({
        //        url: '/Cart/AddToCart',
        //        type: 'post',
        //        dataType: 'json',
        //        data: {
        //            productId: id,
        //            quantity: parseInt($('#txtQuantity').val()),
        //            color: colorId,
        //            size: sizeId
        //        },
        //        success: function () {
        //            tedu.notify('Product was added successful', 'success');
        //            loadHeaderCart();
        //        }
        //    });
        //});
    }

    function searchProduct(cateId, search) {
        //clientshop/product/search/nitiapp?catelogyId=0&keyword=0&sortBy=lastest&pageSize=24
        var href = "/cshop/product/search/nitiapp?catelogyId=" + cateId + "&keyword=" + search +
            "&sortBy=lastest&pageSize=12";
        window.open(href, '_parent');
        return false;
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    function loadData() {
        loadCategoty();
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

}