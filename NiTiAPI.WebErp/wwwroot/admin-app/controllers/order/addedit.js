
var addeditOrderController = function () {

    var userName = $("#hidUserName").val();    

    this.loadTableOrder = function (isPageChanged) {
        loadTableOrder(isPageChanged);
    }

    this.AddEditClearData = function () {
        AddEditClearData();
    }

    this.initialize = function () {
        loadAddEditData();
        registerEvents();
    }

    function registerEvents() {
        formMainValidate();

        $('#btnAddEditSave').on('click', function (e) {            
            var insertOrder = $('#hidInsertOrder').val();
            if (insertOrder === "1") {
                saveOrder(e);
            }
            else if (insertOrder === "2") {
                updateOrder(e);
            }
            else {
                niti.notify(resources["CreateTableError"], "error");
            }
        });   
    }    

    function formMainValidate() {
        //jQuery.validator.addMethod("isCompobox", function (value, element) {
        //    if (value === "%" || value === "0" || value === "")
        //        return false;
        //    else
        //        return true;
        //},
        //    resources["PleaseSelect"]
        //);

        //jQuery.validator.addMethod("isDateVietNam", function (value, element) {
        //    return this.optional(element) || moment(value, "DD/MM/YYYY").isValid();
        //},
        //    resources["DD/MM/YYYY"]
        //);

        //Init validation
        $('#frmMaintainanceAddEdit').validate({
            errorClass: 'red',
            ignore: [],
            language: 'vi',
            rules: {                
                txtAddEditCustomerName: { required: true },
                txtAddEditCustomerAddress: { required: true },
                txtAddEditCustomerEmail: { required: true },
                txtAddEditCustomerPhone: { required: true }                
            }
        });
    }

    function loadTableOrder(isPageChanged) {
        var corporationid = $('#ddlCorporation').val();     

        $.ajax({
            type: "GET",
            url: "/admin/order/GetPaging",
            data: {
                keyword: $('#txtKeyword').val(),
                corporationId: corporationid,              
                pageIndex: niti.configs.pageIndex,
                pageSize: niti.configs.pageSize               
            },
            dataType: "json",
            beforeSend: function () {
                niti.startLoading();
            },
            success: function (response) {
                var template = $('#table-templateOrder').html();
                var render = "";
                if (response.Items.length > 0) {
                    $.each(response.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            CustomerName: item.CustomerName,
                            CorporationName: item.CorporationName,
                            CustomerAddress: item.CustomerAddress,
                            CustomerEmail: item.CustomerEmail,
                            CustomerPhone: item.CustomerPhone,
                            CreateDate: niti.getFormattedDate(item.CreateDate),
                            Status: niti.getStatus(item.Status)
                        });
                    });

                    $("#lblTotalRecords").text(response.TotalRow);
                    if (render !== undefined) {
                        $('#tbl-contentOrder').html(render);
                    }

                    if (response.TotalRow !== 0) {
                        wrapPagingOrder(response.TotalRow, function () {
                            loadTableOrder();
                        },
                            isPageChanged);
                    }
                }
                else {
                    $('#tbl-contentOrder').html('');
                }
                niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }
    function wrapPagingOrder(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / niti.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationULOrder a').length === 0 || changePageSize === true) {
            $('#paginationULOrder').empty();
            $('#paginationULOrder').removeData("twbs-pagination");
            $('#paginationULOrder').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationULOrder').twbsPagination({
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

    function AddEditClearData() {
        $('#hidInsertOrder').val(0);
        $('#hidOrderId').val("");

        $('#ddlAddEditCorporation')[0].selectedIndex = 1; 
        $('#txtAddEditCustomerName').val("");
        $('#txtAddEditCustomerAddress').val("");
        $('#txtAddEditCustomerEmail').val("");
        $('#txtAddEditCustomerNote').val("");
        $('#txtAddEditCustomerPhone').val("");      
    }

    function updateOrder(e) {
        e.preventDefault();

        if ($('#frmMaintainanceAddEdit').valid()) {
            var orderId = $('#hidOrderId').val();

            var corporationId = $('#ddlAddEditCorporation').val();
            var name = $('#txtAddEditCustomerName').val();
            var address = $('#txtAddEditCustomerAddress').val();
            var email = $('#txtAddEditCustomerEmail').val();
            var note = $('#txtAddEditCustomerNote').val();
            var phone = $('#txtAddEditCustomerPhone').val();  

            $.ajax({
                type: "POST",
                url: "/Admin/order/UpdateOrder",
                data: {
                    Id: orderId,
                    CorporationId: corporationId,
                    CustomerName: name,                  
                    CustomerAddress: address,
                    CustomerEmail: email,
                    CustomerNote: note,
                    CustomerPhone: phone,                   
                    CreateBy: userName
                },
                dataType: "json",
                beforeSend: function () {
                    niti.startLoading();
                },
                success: function () {
                    niti.appUserLoginLogger(userName, "Update Order.");
                    niti.notify(resources["CreateTableOK"], 'success');
                    $('#modal-add-edit').modal('hide');
                    AddEditClearData();

                    niti.stopLoading();
                    loadTableOrder(true);
                },
                error: function () {
                    niti.notify(resources["CreateTableError"], 'error');
                    niti.stopLoading();
                }
            });
        }
    }

    function saveOrder(e) {
        e.preventDefault();        

    }

    function loadAddEditData() {

    }

}