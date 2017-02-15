/**
 * Created by PC on 19.12.2016.
 */

$(function () {
    $(".tab").each(function (j) {
        var id = $(this).attr("id"),
            tab_items = $("#" + id + " ul li"),
            tab_itemL = $("#" + id + " ul li").length,
            tab_content = $("#" + id).parent().find(".fk-tab");
        tab_items.filter(":first").addClass("active");
        tab_content.filter(":not(:first)").hide();

        tab_items.click(function () {
            var tab_indis = $(this).index();
            tab_items.removeClass("active").eq(tab_indis).addClass("active");
            tab_content.hide().eq(tab_indis).fadeIn(200);
            return false;
        });
    });
});