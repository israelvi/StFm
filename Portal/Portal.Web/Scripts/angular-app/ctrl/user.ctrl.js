(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('userController', userController);

    //workshopController.$inject = ["connSvc"];
    function userController() {
        var vm = this;
        vm.m = {};
    }
})();
