
//TempData[Message] - dynamic alert in Layout view
document.addEventListener("DOMContentLoaded", function () {
    const alerts = document.querySelectorAll(".alert-dismissible");

    if (alerts.length === 0) return;

    setTimeout(function () {

        alerts.forEach(function (alert) {

            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        });
    }, 4000);
});


// Toggle filter card state and persist it in URL
document.addEventListener("DOMContentLoaded", function () {
    const toggle = document.getElementById("filterCollapseToggle");

    if (!toggle) {
        return;
    }

    toggle.addEventListener("click", function (e) {
        e.preventDefault();

        const url = new URL(window.location.href);
        const currentValue = url.searchParams.get("filtersCollapsed");
        const isCollapsed = currentValue === "true";
        const newValue = (!isCollapsed).toString();

        url.searchParams.set("filtersCollapsed", newValue);

        window.location.href = url.toString();
    });
});


//Pager View - submit form on pageSize change
document.addEventListener("DOMContentLoaded", function () {
    const pageSizeSelect = document.getElementById("selectPager");

    if (!pageSizeSelect) {
        return;
    }

    pageSizeSelect.addEventListener("change", function () {
        const url = new URL(window.location.href);

        url.searchParams.set("pageSize", this.value);
        url.searchParams.set("page", "1");

        window.location.href = url.toString();
    });
});


//Confirm window to delete item in Index actions
document.addEventListener("DOMContentLoaded", function () {
    const deleteForms = document.querySelectorAll(".delete-form");
    const confirmButtons = document.querySelectorAll("[data-confirm-message]");

    deleteForms.forEach(function (form) {
        form.addEventListener("submit", function (e) {
            const button = form.querySelector("button[type='submit']");
            const message = button?.dataset.confirmMessage || "Opravdu chcete položku smazat?";

            if (!confirm(message)) {
                e.preventDefault();
            }
        });
    });

    confirmButtons.forEach(function (button) {
        if (button.closest(".delete-form")) {
            return;
        }

        button.addEventListener("click", function (e) {
            const message = button.dataset.confirmMessage || "Opravdu chcete položku smazat?";

            if (!confirm(message)) {
                e.preventDefault();
            }
        });
    });
});


//remove value form dropdown-filter element when value is equal to "--Vyberte--". Visually looks better for end user
document.addEventListener("DOMContentLoaded", function () {
    const operatorSelects = document.querySelectorAll(".filter-operator");

    function syncFilterValue(operatorSelect) {
        const filterName = operatorSelect.dataset.filterName;
        const valueField = document.querySelector('.filter-value[data-filter-name="' + filterName + '"]');

        if (!valueField) {
            return;
        }

        const isNone = operatorSelect.value === "None";

        if (isNone) {
            valueField.value = "";
            valueField.disabled = true;
        } else {
            valueField.disabled = false;
        }
    }

    operatorSelects.forEach(function (operatorSelect) {
        syncFilterValue(operatorSelect);

        operatorSelect.addEventListener("change", function () {
            syncFilterValue(operatorSelect);
        });
    });
});


// Fake table horizontal scroll script
document.addEventListener("DOMContentLoaded", function () {
    const containers = document.querySelectorAll(".scrollable-table-container");

    containers.forEach(function (container) {
        const topScroll = container.querySelector(".scrollable-table-top");
        const topScrollInner = container.querySelector(".scrollable-table-top-inner");
        const tableWrapper = container.querySelector(".scrollable-table-wrapper");
        const table = container.querySelector(".scrollable-table");

        if (!topScroll || !topScrollInner || !tableWrapper || !table) {
            return;
        }

        function updateTopScrollbar() {
            topScrollInner.style.width = table.scrollWidth + "px";
            topScroll.style.display = table.scrollWidth > tableWrapper.clientWidth ? "block" : "none";
        }

        updateTopScrollbar();

        topScroll.addEventListener("scroll", function () {
            tableWrapper.scrollLeft = topScroll.scrollLeft;
        });

        tableWrapper.addEventListener("scroll", function () {
            topScroll.scrollLeft = tableWrapper.scrollLeft;
        });

        window.addEventListener("resize", updateTopScrollbar);

        if (typeof ResizeObserver !== "undefined") {
            const resizeObserver = new ResizeObserver(updateTopScrollbar);
            resizeObserver.observe(table);
            resizeObserver.observe(tableWrapper);
        }
    });
});