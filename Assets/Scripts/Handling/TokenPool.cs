using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TokenPool : MonoBehaviour
{
    
    [SerializeField] private GameObject blueToken = null;
    [SerializeField] private GameObject greenToken = null;
    [SerializeField] private GameObject orangeToken = null;
    [SerializeField] private GameObject purpleToken = null;
    [SerializeField] private GameObject redToken = null;
    [SerializeField] private GameObject yellowToken = null;
    [SerializeField] private GameObject tokenHolder = null;

    private List<GameObject> availableBlueTokens = new List<GameObject>();
    private List<GameObject> availableGreenTokens = new List<GameObject>();
    private List<GameObject> availableOrangeTokens = new List<GameObject>();
    private List<GameObject> availablePurpleTokens = new List<GameObject>();
    private List<GameObject> availableRedTokens = new List<GameObject>();
    private List<GameObject> availableYellowTokens = new List<GameObject>();
    private Quaternion zeroQuat = new Quaternion (0, 0, 0, 0);
    private Vector3 stashPosition = new Vector3 (0, -2, 0);

    public void PlaceTokenInPool(GameObject aToken, TokenTypes aTokenType)
    {
        switch (aTokenType)
        {
            case TokenTypes.blue:
                this.availableBlueTokens.Add(aToken);
                aToken.GetComponent<Movement>().ResetToken(aPosition: stashPosition);
                break;

            case TokenTypes.green:
                this.availableGreenTokens.Add(aToken);
                aToken.GetComponent<Movement>().ResetToken(aPosition: stashPosition);
                break;

            case TokenTypes.orange:
                this.availableOrangeTokens.Add(aToken);
                aToken.GetComponent<Movement>().ResetToken(aPosition: stashPosition);
                break;

            case TokenTypes.purple:
                this.availablePurpleTokens.Add(aToken);
                aToken.GetComponent<Movement>().ResetToken(aPosition: stashPosition);
                break;

            case TokenTypes.red:
                this.availableRedTokens.Add(aToken);
                aToken.GetComponent<Movement>().ResetToken(aPosition: stashPosition);
                break;

            case TokenTypes.yellow:
                this.availableYellowTokens.Add(aToken);
                aToken.GetComponent<Movement>().ResetToken(aPosition: stashPosition);
                break;

            default:
                break;
        }
    }

    public GameObject GetTokenFromPool(TokenTypes aTokenType)
    {
        GameObject aToken = null;
        
        switch (aTokenType)
        {
            case TokenTypes.blue:
                aToken = ColourPoolManager(aList: this.availableBlueTokens, wantedToken: this.blueToken);
                return aToken;            

            case TokenTypes.green:
                aToken = ColourPoolManager(aList: this.availableGreenTokens, wantedToken: this.greenToken);
                return aToken;

            case TokenTypes.orange:
                aToken = ColourPoolManager(aList: this.availableOrangeTokens, wantedToken: this.orangeToken);
                return aToken;

            case TokenTypes.purple:
                aToken = ColourPoolManager(aList: this.availablePurpleTokens, wantedToken: this.purpleToken);
                return aToken;

            case TokenTypes.red:
                aToken = ColourPoolManager(aList: this.availableRedTokens, wantedToken: this.redToken);
                return aToken;

            case TokenTypes.yellow:
                aToken = ColourPoolManager(aList: this.availableYellowTokens, wantedToken: this.yellowToken);
                return aToken;
            
            default:
            Debug.Log("Returning null");
                return null;
        }
    }

    private GameObject ColourPoolManager(List<GameObject> aList, GameObject wantedToken)
    {
        if (aList.Count > 0)
        {
            GameObject theToken = aList[0];
            aList.RemoveAt(0);
            return theToken;
        }

        else
        {
            GameObject tokenClone = Instantiate(original: wantedToken.gameObject, position: stashPosition, rotation: zeroQuat, parent: tokenHolder.transform);
            return tokenClone;
        }
    }

    private void PopulatePools()
    {
        List<List<GameObject>> listOfPools = new List<List<GameObject>>() {this.availableBlueTokens, 
                                                                        this.availableGreenTokens, 
                                                                        this.availableOrangeTokens, 
                                                                        this.availablePurpleTokens, 
                                                                        this.availableRedTokens, 
                                                                        this.availableYellowTokens};

        List<GameObject> listOfTokens = new List<GameObject>() {this.blueToken, this.greenToken, this.orangeToken, 
                                                                this.purpleToken, this.redToken, this.yellowToken};
        int index = 0;

        foreach(List<GameObject> list in listOfPools)
        {
            for (int x = 0; x < 10; x++)
            {
                list.Add(Instantiate(original: listOfTokens[index], position: this.stashPosition, rotation: this.zeroQuat, parent: this.tokenHolder.transform));
            }

            index += 1;
        }

    }

    private void Awake()
    {
        PopulatePools();
    }





}
