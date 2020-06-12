var homeController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();    
    var corname = $("#hidCorporationName").val();

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

        //$("#btnViewProduct").on('click', function () {
        //    var productId = $(this).data('id');
        //    niti.notify(productId, "success");

        //    // 1 - insert Product
        //    //$('#hidInsertProduct').val(1);
        //    //$('#modal-add-edit').modal('show');
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