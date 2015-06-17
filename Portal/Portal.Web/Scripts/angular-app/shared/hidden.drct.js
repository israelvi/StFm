(function () {
    "use strict";
    angular
        .module(window.constMainApp)
        .directive('ngUpdateHidden', ngUpdateHidden);

    function ngUpdateHidden() {
        return function (scope, el, attr) {
            var model = attr['ngModel'];
            scope.$watch(model, function (nv) {
                el.val(nv);
            });
        };
    };
})();
