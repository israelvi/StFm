(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('loginController', loginController);

    loginController.$inject = ["connSvc"];

    function loginController(connSvc) {
        var vm = this;
        vm.m = {};
        vm.isLogin = true;
        vm.login = login;
        vm.svc = connSvc;
        
        function login (formId, urlToGo) {
            var data = $(formId).serialize();
            vm.svc.post(urlToGo, data).then(function(data) {
                window.location.replace(data.UrlToGo);
            });
            vm.m.password = "";
        }
    }
})();

/*
app.controller('loginController', function () {
    
    $scope.doForget = function (formId, urlToGo) {
        var data = $(formId).serialize();
        $scope.m.emailForgot = "";
        $scope.working = true;
        $http({
            method: 'POST',
            url: urlToGo,
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: data
        }).success($scope.handleSuccessForgot)
            .error($scope.handleErrorForgot);
    };


    $scope.handleSuccessForgot = function (data) {

        if (data === undefined || data === null) {
            $scope.handleErrorForgot();
        }
        else if (data.HasError === false) {
            $scope.msgErrForgot = data.Message;
            $scope.hideMsgForgot();
        }
        else if (data.HasError === true) {
            $scope.msgSucForgot = data.Message;
            $scope.hideMsgForgot();
        }

    };

    $scope.handleErrorForgot = function () {
        $scope.working = false;
        $scope.msgErrForgot = "Ocurrió un error de red. Por favor intente más tarde";
        $scope.hideMsgForgot();
    };
    
    $scope.hideMsgForgot = function () {
        $timeout(function () {
            $scope.msgErrForgot = "";
            $scope.msgSucForgot = "";
        }, 10000);
    };
    //$scope.forgotView = function() {
    //    $scope.isLogin = false;
    //};
});*/