var orderController = function () {

    var userCorporationId = $("#hidUserCorporationId").val();
    var userName = $("#hidUserName").val();

    var addeditOrder = new addeditOrderController();
    var listOrder = new listOrderController();

    this.initialize = function () {
        loadCorporation();
        loadData();
        registerEvents();

        addeditOrder.initialize();
        listOrder.initialize();
    }

    function registerEvents() {
        $('#txtFromDate, #txtToDate').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            language: 'vi'
        });

        $('#txtKeyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                addeditOrder.loadTableOrder();     
            }
        });

        $("#btnSearch").on('click', function () {
            addeditOrder.loadTableOrder();     
        });

        $("#ddl-show-pageOrders").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            addeditOrder.loadTableOrder(true);     
        });

        $('body').on('click', '.btnEditOrder', function (e) {
            e.preventDefault();
            var orderId = $(this).data('id');
            // 2 - Update Order
            $('#hidInsertOrder').val(2);
            loadEditOrder(orderId);
        });

        $('body').on('click', '.btnListOrder', function (e) {
            e.preventDefault();
            var ordertId = $(this).data('id');
            listOrder.loadTableListOrder(ordertId);
            $('#modal-add-editListOrder').modal('show');            
        });

        $('body').on('click', '.btnDeleteOrder', function (e) {
            e.preventDefault();
            var ordertId = $(this).data('id');
            deleteOrder(ordertId);
        });

        $("#btnExportExcel").on('click', function () {
            var fromdate = $('#txtFromDate').val();
            var todate = $('#txtToDate').val();

            var fromDate = niti.getFormatDateYYMMDD(fromdate);
            var toDate = niti.getFormatDateYYMMDD(todate);

            exportExcelOrder(fromDate, toDate);
        });

    }

    function loadData() {
        var dateNow = niti.getFormattedDate(new Date());
        $('#txtFromDate').val(dateNow);        
        $('#txtToDate').val(dateNow);

        addeditOrder.AddEditClearData();
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
                $('#ddlPTOCorporation').html(render);

                if (userCorporationId !== "1") {
                    $('#ddlCorporation').prop('disabled', true);
                    $('#ddlAddEditCorporation').prop('disabled', true);
                    $('#ddlPTOCorporation').prop('disabled', true);
                }
                else {
                    $('#ddlCorporation').prop('disabled', false);
                    $('#ddlAddEditCorporation').prop('disabled', false);
                    $('#ddlPTOCorporation').prop('disabled', false);
                }

                $("#ddlCorporation")[0].selectedIndex = 1;
                $("#ddlAddEditCorporation")[0].selectedIndex = 1;
                $("#ddlPTOCorporation")[0].selectedIndex = 1;

                addeditOrder.loadTableOrder();            
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadEditOrder(orderid) {       
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

                $('#txtAddEditCustomerName').val(order.CustomerName);
                $('#txtAddEditCustomerAddress').val(order.CustomerAddress);
                $('#txtAddEditCustomerEmail').val(order.CustomerEmail);
                $('#txtAddEditCustomerNote').val(order.CustomerNote);
                $('#txtAddEditCustomerPhone').val(order.CustomerPhone);
     
                $('#modal-add-edit').modal('show');
                niti.stopLoading();
            },
            error: function (status) {
                niti.notify(status, 'error');
                niti.stopLoading();
            }
        });
    }

    function deleteOrder(orderid) {
        niti.confirm(resources["DeleteSure"], function () {
            $.ajax({
                type: "POST",
                url: "/Admin/order/DeleteOrder",
                data: {
                    Id: orderid,
                    username: userName
                },
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete Order.");
                    niti.notify(resources["DeleteTableOK"], 'success');
                    niti.stopLoading();
                    addeditOrder.loadTableOrder();    
                },
                error: function (status) {
                    niti.notify('Has an error in deleting progress', 'error');
                    niti.stopLoading();
                }
            });
        });
    }

    function exportExcelOrder(fromDate, toDate) {
        //alert(formDate + ',' + toDate);
        $.ajax({
            type: 'POST',
            url: '/admin/order/ExcelOrderTo',
            data: {
                FromDate: fromDate,
                ToDate: toDate,
                languageId: "vi-VN"
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

}