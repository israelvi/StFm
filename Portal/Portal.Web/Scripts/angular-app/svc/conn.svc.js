(function () {
    "use strict";

    angular
        .module(window.constMainApp)
        .service("connSvc", connSvc);

    connSvc.$inject = ["$http", "$timeout", "$q"];

    function connSvc($http, $timeout, $q) {
        var vm = this;
        vm.m = {};
        vm.post = post;
        vm.handleSuccess = handleSuccess;
        vm.handleError = handleError;
        vm.hideMsgErr = hideMsgErr;

        function post(urlToGo, data) {
            var def = $q.defer();
            vm.working = true;
            $http({
                method: 'POST',
                url: urlToGo,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: data
            }).success(function (res) {
                vm.handleSuccess(res, def);
            }).error(function (res) {
                vm.handleError(res, def);
            });
            return def.promise;
        }

        function handleSuccess(res, def) {
            vm.working = false;

            if (res === undefined || res === null) {
                vm.handleError();
                return;
            }
            else if (res.HasError === true) {
                vm.msgErr = res.Message;
                vm.hideMsgErr();
            }
            else if (res.HasError === false) {
                def.resolve(res);
            }
            def.reject(res);
        };

        function handleError(res, def) {
            vm.working = false;
            vm.msgErr = "Ocurrió un error de red. Por favor intente más tarde";
            vm.hideMsgErr();
            def.reject(res);
        };


        function hideMsgErr() {
            $timeout(function () {
                vm.msgErr = "";
            }, 10000);
        };

    }

})();

