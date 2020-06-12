var homeController = function () {

    //var userCorporationId = $("#hidUserCorporationId").val();
    //var userName = $("#hidUserName").val();    
    var corname = $("#hidCorporationName").val();

    this.initialize = function () {
        registerEvents();
        loadData();
    }

    function registerEvents() {

        $('body').on('click', '.imgViewPostId', function (e) {
            e.preventDefault();
            var postId = $(this).data('id');

            alert(postId);
        });
        $('body').on('click', '.titleViewPostId', function (e) {
            e.preventDefault();
            var postId = $(this).data('id');

            alert(postId);
        });
        $('body').on('click', '.btnViewPost', function (e) {        
            e.preventDefault();
            var postId = $(this).data('id');

            alert(postId);
        });
       

    }

    function loadData() {

    }

}