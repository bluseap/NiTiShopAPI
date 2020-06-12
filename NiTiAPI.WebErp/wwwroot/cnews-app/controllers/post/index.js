var postController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();    
    var corname = $("#hidCorporationName").val();

    var corporationId = $("#hidCnewsPostcorporationId").val();
    var catelogyNewsId = $("#hidCnewscatelogyNewsId").val();

    this.initialize = function () {
        registerEvents();
        loadData();        
    }

    function registerEvents() {

        $("#ddlCategoryNews").on('change', function () {            
            loadPostCategory();
        });

        $("#ddl-show-pagePost").on('change', function () {
            niti.configs.pageSize = $(this).val();
            niti.configs.pageIndex = 1;
            loadPostCategory(true);
        });

        $('body').on('click', '.clickPostId', function (e) {
            e.preventDefault();
            var postId = $(this).data('id');
            alert(postId);
        });

    }

    function loadData() {
        loadCategoryNews(1);
        loadPostCategoryNewsId(catelogyNewsId);
    }

    function loadCategoryNews(corporationid) {
        return $.ajax({
            type: 'GET',
            url: '/cnews/Post/GetCoporationId',
            data: { cororationId: corporationid },
            dataType: 'json',
            success: function (response) {
                var choosen = resources["Choose"];
                var render = "<option value='0' >-- " + choosen + " --</option>";
                $.each(response.Items, function (i, item) {
                    render += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                });
                $('#ddlCategoryNews').html(render);  

                $('#ddlCategoryNews').val(catelogyNewsId);
            },
            error: function () {
                niti.notify(resources['NotFound'], 'error');
            }
        });
    }

    function loadPostCategory(isPageChanged) {
        var corporationid = 1;
        var catalogid = $('#ddlCategoryNews').val();

        $.ajax({
            type: "GET",
            url: "/cnews/post/GetPaging",
            data: {
                keyword: "%",
                culture: "vi-VN",
                corporationId: corporationid,
                categoryNewsId: catalogid,
                pageIndex: niti.configs.pageIndex,
                pageSize: niti.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                //niti.startLoading();
            },
            success: function (response) {
                var template = $('#table-templatePostCategory').html();
                var render = "";
                if (response.Items.length > 0) {
                    $.each(response.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Title: item.Title,
                            CategoryNewsName: item.CategoryNewsName,
                            CorporationName: item.CorporationName,
                            Description: item.Description,
                            Image: item.Image === null ? '<img src="/admin-side/images/user.png" width=300 height=200 alt=""' : '<img src="' + item.Image + '" width=300 height=200 alt=""/>'
                            //Status: niti.getStatusPosts(item.Status)
                        });
                    });

                    $("#lblTotalRecords").text(response.TotalRow);
                    if (render !== undefined) {
                        $('#tbl-contentPostCategory').html(render);
                    }

                    if (response.TotalRow !== 0) {
                        wrapPagingPostCategory(response.TotalRow, function () {
                            loadPostCategory();
                        },
                            isPageChanged);
                    }
                }
                else {
                    $('#tbl-contentPostCategory').html('');
                }
                //niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }
    function wrapPagingPostCategory(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / niti.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationULPost a').length === 0 || changePageSize === true) {
            $('#paginationULPost').empty();
            $('#paginationULPost').removeData("twbs-pagination");
            $('#paginationULPost').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationULPost').twbsPagination({
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

    function loadPostCategoryNewsId(catelogynewsid) {
        var corporationid = 1;
        var catalogid = catelogynewsid;

        $.ajax({
            type: "GET",
            url: "/cnews/post/GetPaging",
            data: {
                keyword: "%",
                culture: "vi-VN",
                corporationId: corporationid,
                categoryNewsId: catalogid,
                pageIndex: niti.configs.pageIndex,
                pageSize: niti.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                //niti.startLoading();
            },
            success: function (response) {
                var template = $('#table-templatePostCategory').html();
                var render = "";
                if (response.Items.length > 0) {
                    $.each(response.Items, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.Id,
                            Title: item.Title,
                            CategoryNewsName: item.CategoryNewsName,
                            CorporationName: item.CorporationName,
                            Description: item.Description,
                            Image: item.Image === null ? '<img src="/admin-side/images/user.png" width=300 height=200 alt=""' : '<img src="' + item.Image + '" width=300 height=200 alt=""/>'
                            //Status: niti.getStatusPosts(item.Status)
                        });
                    });

                    $("#lblTotalRecords").text(response.TotalRow);
                    if (render !== undefined) {
                        $('#tbl-contentPostCategory').html(render);
                    }

                    if (response.TotalRow !== 0) {
                        wrapPagingPostCategory(response.TotalRow, function () {
                            loadPostCategoryNewsId(catalogid);
                        },
                            isPageChanged);
                    }
                }
                else {
                    $('#tbl-contentPostCategory').html('');
                }
                //niti.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }

}