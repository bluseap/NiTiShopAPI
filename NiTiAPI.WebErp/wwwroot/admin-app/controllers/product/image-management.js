
var imageproductController = function () {

    var userName = $("#hidUserName").val();
    var imageManagement = [];

    this.loadImageProduct = function (productid) {
        loadImageProduct(productid);
    }

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {       

        $("#fileImageManagement").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;

            var data = new FormData();

            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
           
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImageProduct",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearfileImageManagement($("#fileImageManagement"));                
                    imageManagement.push(path);

                    saveImageManagement(path);

                    $('#ImageManagement-list').append('<div class="col-md-3"><img width="100"  data-path="' + path + '" src="' + path + '"></div>');
                    niti.notify(resources["UploadFile"], 'success');
                },
                error: function () {
                    niti.notify(resources["UploadFileError"], 'error');
                }
            }); 
        });

        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            $(this).closest('div').remove();           
          
            var productimageId = $(this).data("id");     
           
            $.ajax({
                url: '/admin/Product/DeleteImage',
                data: {
                    productImageId: productimageId,                 
                    username: userName
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    niti.appUserLoginLogger(userName, "Delete Images at Image Management.");
                    $('#modal-image-manage').modal('hide');
                    $('#ImageManagement-list').html('');
                    clearfileImageManagement($("#fileImageManagement"));
                }
            });
        });

    }

    function clearfileImageManagement(ctrl) {
        try {
            imageManagement = [];
            ctrl.value = null;
            ctrl.value('');
        }
        catch (status) {
            console.log(status);
        }
    }

    function saveImageManagement(imageManagement) {
        var productid = $('#hidProductId').val();
        $.ajax({
            url: '/admin/Product/SaveImages',
            data: {
                productId: productid,
                images: imageManagement,
                username: userName
            },
            type: 'post',
            dataType: 'json',
            success: function (response) {
                niti.appUserLoginLogger(userName, "Save Images at Image Management.");
                $('#modal-image-manage').modal('hide');
                $('#ImageManagement-list').html('');
                clearfileImageManagement($("#fileImageManagement"));
                imageManagement = [];
            }
        });
    }

    function loadImageProduct(productid) {
        $.ajax({
            url: '/admin/Product/GetProductImages',
            data: {
                productId: productid
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                $.each(response, function (i, item) {
                   render += '<div class="col-md-3"><img width="100" src="' + item.Path
                        + '" /><br/><a href="#" class="btn-delete-image" data-id="' + item.Id + '" >Xóa</a></div>';                    
                });

                $('#ImageManagement-list').html(render);
            }
        });
    }

}