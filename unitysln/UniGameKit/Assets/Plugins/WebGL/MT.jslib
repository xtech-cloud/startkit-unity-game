mergeInto(LibraryManager.library, {

     QueryUUID: function () {
        var reg = new RegExp("(^|&)"+ "uuid" +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        var uuid = "";
        if(r!=null)
          uuid = unescape(r[2]); 
        else 
          uuid = "";
        var bufferSize = lengthBytesUTF8(uuid) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(uuid, buffer, bufferSize);
        return buffer;
    },
});
