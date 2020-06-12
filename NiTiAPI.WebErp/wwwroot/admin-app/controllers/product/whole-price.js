
var wholepriceController = function () {

    var userName = $("#hidUserName").val();

    this.loadWholePrice = function (productid) {
        loadWholePrice(productid);
    }

    this.initialize = function () {
        registerEvents();
        wholepriceClearData();
    }

    function registerEvents() {
        $("#btnSaveWholePrice").on('click', function () {
            var insertWholePrice = $('#hidInsertWholePrice').val();

            if (insertWholePrice === "1") // insert
            {
                saveWholePrice();
            }
            else if (insertWholePrice === "2") // update
            {
                updateWholePrice();
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }
        });  

        $('#btn-add-whole-price').on('click', function () {
            var template = $('#template-table-whole-price').html();
            var render = Mustache.render(template, {
                Id: 0,
                FromQuantity: 0,
                ToQuantity: 0,
                Price: 0
            });
            $('#table-content-whole-price').append(render);
        });

        $('body').on('click', '.btn-delete-whole-price', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
    }   

    function loadWholePrice(productid) {
        $.ajax({
            url: '/admin/Product/GetProductWholePrice',
            data: {
                productId: productid
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.length === 0) {
                    $('#hidInsertWholePrice').val(1); // insert
                }
                else {
                    $('#hidInsertWholePrice').val(2); // update
                }

                var render = '';
                var template = $('#template-table-whole-price').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        FromQuantity: item.FromQuantity,
                        ToQuantity: item.ToQuantity,
                        Price: item.Price
                    });
                });
                $('#table-content-whole-price').html(render);
            }
        });
    }

    function wholepriceClearData() {
        $('#hidInsertWholePrice').val(0);
    }    

    function saveWholePrice() {
        var priceList = [];
        $.each($('#table-content-whole-price').find('tr'), function (i, item) {
            priceList.push({
                Id: $(item).data('id'),
                ProductId: $('#hidProductId').val(),
                FromQuantity: $(item).find('input.txtQuantityFrom').first().val(),
                ToQuantity: $(item).find('input.txtQuantityTo').first().val(),
                Price: $(item).find('input.txtWholePrice').first().val(),
            });
        });

        var xml = '';
        xml = xml + "<tables>";
        for (var i = 0; i < priceList.length; i++) {
            var listfield = priceList[i];
            xml += "<items>";
            xml += '<Id>' + listfield.Id + '</Id>';
            xml += '<ProductId>' + listfield.ProductId + '</ProductId>';
            xml += '<FromQuantity>' + listfield.FromQuantity + '</FromQuantity>';
            xml += '<ToQuantity>' + listfield.ToQuantity + '</ToQuantity>';
            xml += '<Price>' + listfield.Price + '</Price>';
            xml += "</items>";
        }
        xml = xml + '</tables>'; 

        $.ajax({
            url: '/admin/Product/SaveWholePrice',
            data: {
                productWholePriceXML: xml,
                username: userName,
                languageId: "vi-VN"                
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                niti.appUserLoginLogger(userName, "Save Product Whole Prices Management.");
                niti.stopLoading();
                wholepriceClearData();
                $('#modal-whole-price').modal('hide');
                $('#table-content-whole-price').html('');
            }
        });
    }

    function updateWholePrice() {
        var priceList = [];
        $.each($('#table-content-whole-price').find('tr'), function (i, item) {
            priceList.push({
                Id: $(item).data('id'),
                ProductId: $('#hidProductId').val(),
                FromQuantity: $(item).find('input.txtQuantityFrom').first().val(),
                ToQuantity: $(item).find('input.txtQuantityTo').first().val(),
                Price: $(item).find('input.txtWholePrice').first().val(),
            });
        });

        var xml = '';
        xml = xml + "<tables>";
        for (var i = 0; i < priceList.length; i++) {
            var listfield = priceList[i];
            xml += "<items>";
            xml += '<Id>' + listfield.Id + '</Id>';
            xml += '<ProductId>' + listfield.ProductId + '</ProductId>';
            xml += '<FromQuantity>' + listfield.FromQuantity + '</FromQuantity>';
            xml += '<ToQuantity>' + listfield.ToQuantity + '</ToQuantity>';
            xml += '<Price>' + listfield.Price + '</Price>';
            xml += "</items>";
        }
        xml = xml + '</tables>';

        $.ajax({
            url: '/admin/Product/SaveWholePrice',
            data: {
                productWholePriceXML: xml,
                username: userName,
                languageId: "vi-VN"
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                niti.appUserLoginLogger(userName, "Update or delete Product Whole Prices Management.");
                niti.stopLoading();
                wholepriceClearData();
                $('#modal-whole-price').modal('hide');
                $('#table-content-whole-price').html('');
            }
        });
    }

}