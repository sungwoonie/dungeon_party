using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking_Board : SingleTon<Ranking_Board>
{
    public Transform cpp_ranking_content_list;
    public Transform lv_ranking_content_list;

    public GameObject lock_object;

    private Ranking_Content[] cpp_ranking_contents;
    private Ranking_Content[] lv_ranking_contents;

    private Ranking_Content my_cpp_ranking_content;
    private Ranking_Content my_lv_ranking_content;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        cpp_ranking_contents = cpp_ranking_content_list.GetComponentsInChildren<Ranking_Content>(true);
        lv_ranking_contents = lv_ranking_content_list.GetComponentsInChildren<Ranking_Content>(true);

        foreach (var cpp_ranking_content in cpp_ranking_contents)
        {
            if (cpp_ranking_content.rank == 0)
            {
                my_cpp_ranking_content = cpp_ranking_content;
                break;
            }
        }

        foreach (var lv_ranking_content in lv_ranking_contents)
        {
            if (lv_ranking_content.rank == 0)
            {
                my_lv_ranking_content = lv_ranking_content;
                break;
            }
        }
    }

    #endregion

    #region "Set"

    public void Set_Lock_Object(bool is_on)
    {
        if (lock_object.activeSelf)
        {
            lock_object.SetActive(is_on);
        }
    }

    public void Set_Ranking_Board(bool cpp, List<Ranking> ranking_list, Ranking my_rank)
    {
        Ranking_Content[] target_contents = cpp ? cpp_ranking_contents : lv_ranking_contents;
        Ranking_Content my_ranking_content = cpp ? my_cpp_ranking_content : my_lv_ranking_content;

        for (int i = 0; i < target_contents.Length; i++)
        {
            if (i > ranking_list.Count)
            {
                target_contents[i].gameObject.SetActive(false);
            }
            else
            {
                foreach (var ranking in ranking_list)
                {
                    if (target_contents[i].rank == ranking.rank)
                    {
                        target_contents[i].Set_Ranking_Content(ranking, cpp);
                        target_contents[i].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        my_ranking_content.Set_Ranking_Content(my_rank, cpp);
    }

    #endregion
}
