function test(element)
{
  //alert(element.children[0].checked);
  alert(element);
}

function hideElement(elementId) {
  document.getElementById(elementId).style.display = 'none';
}
function showElement(elementId) {
  document.getElementById(elementId).style.display = 'initial';
}

function textBoxContainsValue(element, elementToShow, elementToHide) {
  if (element.value != '') {
    showElement(elementToShow);
    hideElement(elementToHide);
  }
  else {
    showElement(elementToHide);
    hideElement(elementToShow);
  }
}

function textBoxContainsValueGlobal(elementClassName, elementToShow, elementToHide) {
  var theElement = document.getElementsByClassName(elementClassName);
  textBoxContainsValue(theElement[0], elementToShow, elementToHide);
}


function showElementById(element, elementToShowId) {
  if (element.checked) {
    document.getElementById(elementToShowId).style.display = 'initial';
  }
  else {
    document.getElementById(elementToShowId).style.display = 'none';
  }
}

function showElementGlobal(elementId, showElementId) {
  if (document.getElementById(elementId).checked) {
    //document.getElementById(showElementId).style.visibility = 'visible';
    document.getElementById(showElementId).style.display = 'initial';
  }
  else {
    //document.getElementById(showElementId).style.visibility = 'hidden';
    document.getElementById(showElementId).style.display = 'none';
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