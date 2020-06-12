var orderController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();      

    var corporationName = $("#hidOrderCorporationName").val();

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {

        $('#bntBeginOrder').on('click', function (e) {
            e.preventDefault();
            beginOrder();
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

    }

    function loadData() {        
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

    function beginOrder() {
        var name = $("#CustomerName").val();
        var email = $("#CustomerEmail").val();
        var phone = $("#phoneNumber").val();
        var address = $("#address").val();

        $.ajax({
            url: '/cshop/Cart/BeginOrder',
            type: 'post',
            data: {
                corporationname: corporationName,
                customerName: name,
                customerEmail: email,
                customerPhone: phone,
                customerAddress: address
            },
            dataType: 'json',            
            success: function () {
                //niti.notify('Order was added successful', 'success');  
                var href = "/cshop/cart/orderinfo/" + corporationName;
                window.open(href, '_parent');
            }
        });
    }

}