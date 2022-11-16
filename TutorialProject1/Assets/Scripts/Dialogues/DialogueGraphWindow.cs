#if UNITY_EDITOR
using System.Linq;
using Subtegral.DialogueSystem.DataContainers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphWindow : EditorWindow
{    
    private DialogueGraphView graphView;
    private string fileName = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window  = GetWindow<DialogueGraphWindow>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable() 
    {
        AddGraphView(); 
        GenerateToolbar();
        AddStyles(); 
        GenerateBlackBoard();  
    }

    private void AddGraphView() //ConstructGraphView
    {
        graphView = new DialogueGraphView{ name = "Dialogue Graph" };
        graphView.StretchToParentSize();  

        rootVisualElement.Add(graphView);
    }
    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name");
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        //кнопка сохранить данные
        Button saveDataButton = new Button( () => SaveData());
        saveDataButton.text = "Save data";
        toolbar.Add(saveDataButton);

        //кнпока загрузить данные
        Button loadDataButton = new Button( () => LoadData());
        loadDataButton.text = "Load data";
        toolbar.Add(loadDataButton);

        //кнопка создать новый ноуд
        Button nodeCreateButton = new Button( () => { graphView.AddNodeElement("", false); } );
        nodeCreateButton.text = "Create Start Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void LoadData()
    {
        if (string.IsNullOrEmpty(fileName))
            EditorUtility.DisplayDialog("","Неподходящее имя файла", "Ок");

        var saveUtility = GraphSaveUtility.GetInstance(graphView);
        saveUtility.LoadGraph(fileName);
    }

    private void SaveData()
    {
        if (string.IsNullOrEmpty(fileName))
            EditorUtility.DisplayDialog("","Неподходящее имя файла", "Ок");

        var saveUtility = GraphSaveUtility.GetInstance(graphView);
        saveUtility.SaveGraph(fileName);
    }

    private void GenerateBlackBoard()
    {
        var blackboard = new Blackboard(graphView);
        blackboard.Add(new BlackboardSection {title = "Exposed Variables"});
        blackboard.addItemRequested = _blackboard =>
        {
            graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
        };
        blackboard.editTextRequested = (_blackboard, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField) element).text;
            if (graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                    "OK");
                return;
            }

            var targetIndex = graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
            graphView.ExposedProperties[targetIndex].PropertyName = newValue;
            ((BlackboardField) element).text = newValue;
        };
        blackboard.SetPosition(new Rect(10,30,200,100));
        graphView.Add(blackboard);
        graphView.Blackboard = blackboard;
    }

    private void AddStyles()
    {
        StyleSheet styleSheet = EditorGUIUtility.Load("Styles/VariablesStyle.uss") as StyleSheet;
        rootVisualElement.styleSheets.Add(styleSheet);
    }

    private void OnDisable() 
    {
        rootVisualElement.Remove(graphView); 
    }
}
#endif
