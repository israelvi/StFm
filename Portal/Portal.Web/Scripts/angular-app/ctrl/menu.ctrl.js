(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .controller('menuController', menuController);

    function menuController() {
        var vm = this;
        vm.m = {};
    }
})();