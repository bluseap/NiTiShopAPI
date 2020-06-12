
var quantityController = function () {

    var userName = $("#hidUserName").val();
    var cachedObj = {
        colors: [],
        sizes: []
    };

    this.loadQuantityProduct = function (productid) {
        loadQuantityProduct(productid);
    }

    this.initialize = function () {
        loadAttributeSize();
        loadColors();        
        registerEvents();
        quantitiesClearData();
    }

    function registerEvents() {
        $("#btnSaveQuantity").on('click', function () {      
            var insertQuantites = $('#hidInsertProductQuantities').val(); 

            if (insertQuantites === "1") // insert
            {
                saveQuantity();    
            }
            else if (insertQuantites === "2") // update
            {
                updateQuantity();
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }                     
        });        

        $('#btn-add-quantity').on('click', function () {
            var attributeSize = $("#ddlAttributeSize").val();

            if (attributeSize !== "0") {
                var template = $('#template-table-quantity').html();
                var render = Mustache.render(template, {
                    Id: 0,
                    Colors: getColorOptions(null),
                    Sizes: getSizeOptions(null),
                    Quantity: 0
                });
                $('#table-quantity-content').append(render);
            }
            else {
                niti.notify(resources["BeforeAdd"], "error");
            }
        });

        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();            
        });

        $("#ddlAttributeSize").on('change', function () {
            var attributeId = $("#ddlAttributeSize").val();
            loadSizes(attributeId);
        });
      
    }

    function quantitiesClearData() {
        $('#hidInsertProductQuantities').val(0);
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
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Value + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Value + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function saveQuantity() {
        var quantityList = [];
        $.each($('#table-quantity-content').find('tr'), function (i, item) {
            quantityList.push({
                Id: $(item).data('id'),
                ProductId: $('#hidProductId').val(),
                Quantity: $(item).find('input.txtQuantity').first().val(),
                SizeId: $(item).find('select.ddlSizeId').first().val(),
                ColorId: $(item).find('select.ddlColorId').first().val()
            });
        });

        var xml = '';
        xml = xml + "<tables>";
        for (var i = 0; i < quantityList.length; i++) {
            var listfield = quantityList[i];
            xml += "<items>";
            xml += '<Id>' + listfield.Id + '</Id>';
            xml += '<ProductId>' + listfield.ProductId + '</ProductId>';
            xml += '<Quantity>' + listfield.Quantity + '</Quantity>';
            xml += '<SizeId>' + listfield.SizeId + '</SizeId>';
            xml += '<ColorId>' + listfield.ColorId + '</ColorId>';         
            xml += "</items>";
        }
        xml = xml + '</tables>'; 

        $.ajax({
            type: "POST",
            url: '/admin/Product/SaveQuantities',
            data: {
                productQuantiesXML: xml,
                username: userName,
                languageId: "vi-VN"
            },          
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function () {
                niti.appUserLoginLogger(userName, "Save Quantities at Quantities Management.");
                niti.stopLoading();
                quantitiesClearData();
                $('#modal-quantity-management').modal('hide');
                $('#table-quantity-content').html('');
            }
        });
    }

    function updateQuantity() {
        var quantityList = [];
        $.each($('#table-quantity-content').find('tr'), function (i, item) {
            quantityList.push({
                Id: $(item).data('id'),
                ProductId: $('#hidProductId').val(),
                Quantity: $(item).find('input.txtQuantity').first().val(),
                SizeId: $(item).find('select.ddlSizeId').first().val(),
                ColorId: $(item).find('select.ddlColorId').first().val()
            });
        });

        var xml = '';
        xml = xml + "<tables>";
        for (var i = 0; i < quantityList.length; i++) {
            var listfield = quantityList[i];
            xml += "<items>";
            xml += '<Id>' + listfield.Id + '</Id>';
            xml += '<ProductId>' + listfield.ProductId + '</ProductId>';
            xml += '<Quantity>' + listfield.Quantity + '</Quantity>';
            xml += '<SizeId>' + listfield.SizeId + '</SizeId>';
            xml += '<ColorId>' + listfield.ColorId + '</ColorId>';
            xml += "</items>";
        }
        xml = xml + '</tables>';

        $.ajax({
            type: "POST",
            url: '/admin/Product/SaveQuantities',
            data: {
                productQuantiesXML: xml,
                username: userName,
                languageId: "vi-VN"
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function () {
                niti.appUserLoginLogger(userName, "Update or delete Quantities at Quantities Management.");
                niti.stopLoading();
                quantitiesClearData();
                $('#modal-quantity-management').modal('hide');
                $('#table-quantity-content').html('');
            }
        });
    }

    function loadQuantityProduct(productid) {
        $.ajax({
            url: '/admin/AttributeOptionValue/GetListAttributeSize',
            data: {
                productId: productid,
                language: "vi-VN"
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {              
                cachedObj.sizes = response;
                loadQuantityProductSize(productid);
            }
        });        
    }

    function loadQuantityProductSize(productid) {
        $.ajax({
            url: '/admin/Product/GetProductQuantities',
            data: {
                productId: productid
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                if (response.length === 0) {
                    $('#hidInsertProductQuantities').val(1); // insert
                }
                else {
                    $('#hidInsertProductQuantities').val(2); // update                   
                }

                var render = '';
                var template = $('#template-table-quantity').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Colors: getColorOptions(item.AttributeOptionValueIdColor),
                        Sizes: getSizeOptions(item.AttributeOptionValueIdSize),
                        Quantity: item.Quantity
                    });
                });
                $('#table-quantity-content').html(render);
            }
        });
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

    

}