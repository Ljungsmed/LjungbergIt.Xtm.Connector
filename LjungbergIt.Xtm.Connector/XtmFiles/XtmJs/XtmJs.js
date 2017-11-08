function test(element)
{
  //alert(element.children[0].checked);
  alert('testing');
}

function showElementById(element, elementToShowId) {
  if (element.checked) {
    document.getElementById(elementToShowId).style.display = 'block';
  }
  else {
    document.getElementById(elementToShowId).style.display = 'none';
  }
}

function showElementGlobal(elementId, showElementId) {
  if (document.getElementById(elementId).checked) {
    document.getElementById(showElementId).style.visibility = 'visible';
  }
  else {
    document.getElementById(showElementId).style.visibility = 'hidden';
  }
}

function ToggleAllSubitemsRelatedItems(element)
{
  if (element.checked) {
  }
  else
  {
    var subItemsRelatedSpans = document.getElementsByClassName('classsubitemrelateditem');
    
    numberOfSpans = subItemsRelatedSpans.length;
    for (i = 0; i < numberOfSpans; i++)
    {
      subItemsRelatedSpans[i].childNodes[0].checked = '';
    }
  }
}

function toggleRelatedItem(element) {

  var subItemRelatedCheckbox = document.getElementById('inputchbIncludeAllSubItemsRelatedItems');
  if (subItemRelatedCheckbox.checked) {

    var subItemsRelatedSpans = document.getElementsByClassName('classsubitemrelateditem');
    numberOfSpans = subItemsRelatedSpans.length;
    var itemId = element.childNodes[0].value;

    for (i = 0; i < numberOfSpans; i++) {
      var relatedValue = subItemsRelatedSpans[i].childNodes[0].value;
      if (String(relatedValue).includes(itemId)) {
        if (element.childNodes[0].checked) {
          subItemsRelatedSpans[i].childNodes[0].disabled = false;
        }
        else {
          subItemsRelatedSpans[i].childNodes[0].checked = '';
          subItemsRelatedSpans[i].childNodes[0].disabled = true;
        }
      }
    }
  }
}