(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('processingController', processingController);

    processingController.$inject = ["$scope", "sharedSvc"];

    function processingController($scope, sharedSvc) {
        var vm = this;
        vm.sharedSvc = sharedSvc;
        vm.count = 0;

        $scope.$watch('sharedSvc.vm.cfgProc.procCount', function (count) {
            vm.count = count;
        });
    };
})();

(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('messageController', messageController);

    messageController.$inject = ["$scope", "$sce", "sharedSvc"];

    function messageController($scope, $sce, sharedSvc) {
        var vm = this;
        vm.sharedSvc = sharedSvc;
        vm.ok = ok;

        $scope.$watch('vm.sharedSvc.cfgMsg', function (cfg) {
            vm.title = cfg.title;
            vm.message = $sce.trustAsHtml(cfg.message);
            vm.type = cfg.type;
        });

        function ok() {
            vm.IsOk = true;
            vm.sharedSvc.hideMsg($scope);
        }
    };
})();


(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('confirmationController', confirmationController);

    confirmationController.$inject = ["$scope", "$sce", "sharedSvc"];

    function confirmationController($scope, $sce, sharedSvc) {
        var vm = this;
        vm.sharedSvc = sharedSvc;
        vm.yes = yes;
        vm.no = no;

        $scope.$watch('vm.sharedSvc.cfgMsg', function (cfg) {
            vm.title = cfg.title;
            vm.message = $sce.trustAsHtml(cfg.message);
            vm.type = cfg.type;
        });

        function yes() {
            vm.IsOk = true;
            sharedSvc.hideMsg($scope);
        }

        function no() {
            vm.IsOk = false;
            sharedSvc.hideMsg($scope);
        }
    };
})();

