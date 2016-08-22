$(function () {
    "use strict";

    $(".quality-info").click(function (e) {
        var clickedItem = $(e.currentTarget);
        var isSelected = clickedItem.hasClass("quality-info_selected");
        if (isSelected) {
            return;
        }

        $(".quality-info").removeClass("quality-info_selected");
        clickedItem.addClass("quality-info_selected");

        var clickedQualityDescription = clickedItem.find(".quality-info__description").text();
        $(".selected-quality-description").text(clickedQualityDescription);
    });
});